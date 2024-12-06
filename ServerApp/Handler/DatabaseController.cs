using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ServerApp.ConstValue;
using ServerApp.Models;

namespace ServerApp.Handler
{
    internal static class DatabaseController
    {

        private const float CHANCE_OF_BIRTHDAY_INSTRUCTOR = 0.85f;
        private const float CHANCE_OF_BIRTHDAY_STUDENT = 0.7f;

        private static Random random = new Random();

        private const float COURSE_RATIO = 0.1f;
        private const float DEPARTMENT_RATIO = 0.001f;
        private const float GROUP_RATIO = 0.03f;
        private const float INSTRUCTOR_RATIO = 0.05f;
        private const float STUDENT_RATIO = 1f;

        static DatabaseController()
        {

            ApplicationContext context = new ApplicationContext();
            context.Database.Migrate();
            context.SaveChanges();

        }

        private static T getRandom<T>(T[] arr) => arr[random.Next(arr.Length)];

        private static DateTime startStudentDate = new DateTime(2000, 1, 1);
        private static DateTime endStudentDate = new DateTime(2010, 12, 31);

        private static DateTime startInstructorDate = new DateTime(1970, 1, 1);
        private static DateTime endInstructorDate = new DateTime(2000, 12, 31);

        static DateTime GetRandom(DateTime start, DateTime end)
        {
            int range = (end - start).Days;
            return start.AddDays(random.Next(range));
        }

        public static int GetStudentCount { get => new ApplicationContext().Students.Count(); }
        public static int GetInstructorCount { get => new ApplicationContext().Instructors.Count(); }
        public static int GetGroupCount { get => new ApplicationContext().Groups.Count(); }
        public static int GetEnrollmentCount { get => new ApplicationContext().Enrollments.Count(); }
        public static int GetDepartmentCount { get => new ApplicationContext().Departments.Count(); }
        public static int GetCourseCount { get => new ApplicationContext().Courses.Count(); }

        public static bool CreateData(int count)
        {

            ApplicationContext context = new ApplicationContext();

            try
            {

                Logger.LogDebug("Create Department table");

                    for (int i = 0; i < count * DEPARTMENT_RATIO; i++)
                    {
                        var name = getRandom(DepartmentNames.Names);
                        context.Departments.Add(new Department()
                        {
                            DepartmentName = name
                        });
                    }

                Logger.LogDebug("Save Department table");
                if (context.SaveChanges() == 0) return false;
                Logger.LogDebug("Create Instructor table");

                var deps = context.Departments.ToArray();

                for (int i = 0; i < count * INSTRUCTOR_RATIO; i++)
                {

                    var firstName = getRandom(PersonNames.FirstNames);
                    var LastName = getRandom(PersonNames.LastNames);
                    var MiddleName = getRandom(PersonNames.MiddleNames);
                    int depId = getRandom(deps).DepartmentId;
                    var date = random.NextDouble() > CHANCE_OF_BIRTHDAY_INSTRUCTOR ? default : GetRandom(startInstructorDate, endInstructorDate);

                    context.Instructors.Add(new Instructor()
                    {

                        FirstName = firstName,
                        LastName = LastName,
                        DepartmentId = depId,
                        DateOfBirth = date,

                    });

                }

                Logger.LogDebug("Save Instructor table");
                if (context.SaveChanges() == 0) return false;
                Logger.LogDebug("Create Course table");

                var insts = context.Instructors.ToArray();

                for (int i = 0; i < count * COURSE_RATIO; i++)
                {

                    var name = getRandom(CourseNames.Courses);
                    int instId = getRandom(insts).InstructorId;

                    context.Courses.Add(new Course()
                    {

                        CourseName = name,
                        InstructorId = instId

                    });

                }

                Logger.LogDebug("Save Course table");
                if (context.SaveChanges() == 0) return false;
                Logger.LogDebug("Create Group table");
                {

                    int course = 1;
                    int dep = 1;
                    int groupName = 1;

                    int maxCount = (int)(count * GROUP_RATIO);
                    int maxCoursesCount = maxCount / (6 * 9);
                    maxCoursesCount = maxCoursesCount > 99 ? 99 : maxCoursesCount;

                    for (int i = 0; i < maxCoursesCount * 6 * 9; i++)
                    {

                        context.Groups.Add(new Group()
                        {

                            CurrentCourse = course,
                            DepartmentNumber = dep,
                            GroupNumber = groupName

                        });

                        course++;
                        if (course == 7)
                        {
                            dep++;
                            course = 1;
                        }
                        if (dep == 10)
                        {
                            groupName++;
                            dep = 1;
                        }

                    }

                }

                Logger.LogDebug("Save Group table");
                if (context.SaveChanges() == 0) return false;
                Logger.LogDebug("Create Student table");

                var gros = context.Groups.ToArray();

                for (int i = 0; i < count * STUDENT_RATIO; i++)
                {

                    var firstName = getRandom(PersonNames.FirstNames);
                    var LastName = getRandom(PersonNames.LastNames);
                    int groupId = getRandom(gros).GroupId;
                    var date = random.NextDouble() > CHANCE_OF_BIRTHDAY_STUDENT ? default : GetRandom(startStudentDate, endStudentDate);

                    context.Students.Add(new Student()
                    {

                        FirstName = firstName,
                        LastName = LastName,
                        GroupId = groupId,
                        DateOfBirth = date

                    });

                }

                Logger.LogDebug("Save Student table");
                if (context.SaveChanges() == 0) return false;
                Logger.LogDebug("Create Enrollment table");

                var cous = context.Courses.ToArray();
                var stus = context.Students.ToArray();
                var len = Enum.GetValues<Grade>().Length;
                var stusCount = context.Students.Count();

                for (int i = 0; i < stusCount; i++)
                {

                    int courseId = getRandom(cous).CourseId;
                    int studentId = getRandom(stus).StudentId;
                    int grade = random.Next(len);

                    context.Enrollments.Add(new Enrollment()
                    {

                        CourseId = courseId,
                        Grade = (Grade)grade,
                        StudentId = studentId

                    });

                }

                Logger.LogDebug("Save Enrollment table");
                if (context.SaveChanges() == 0) return false;
                return true;

            }
            catch (Exception ex)
            {

                Logger.LogError(ex.Message);
                Logger.LogErrorTrace(ex.StackTrace!);

                return false;

            }

        }

        public static int FindArithmeticMeanValues(int start, int countArr)
        {

            ApplicationContext context = new ApplicationContext();
            int globalGreade = 0;

            var stus = context.Students.Include(s => s.Enrollments).Skip(start).Take(countArr);

            foreach (var student in stus)
            {

                if(student.Enrollments.Count == 0) continue;

                int grade = 0;
                foreach (var e in student.Enrollments)
                    grade += (int)e.Grade!;

                grade /= student.Enrollments.Count;
                globalGreade += grade;

            }

            globalGreade /= context.Students.Count();
            
            return globalGreade;

        }

        public static int CountAllGroups(int start, int countArr)
        {

            ApplicationContext context = new ApplicationContext();

            int count = 0;
            var grus = context.Groups.Include(s => s.Students).Skip(start).Take(countArr);
            foreach (var group in grus)
                count += group.Students.Count;

            count /= context.Groups.Count();

            return count;

        }

        public static bool ChangeCourseToAll(int start, int countArr)
        {

            ApplicationContext context = new ApplicationContext();
            try
            {

                var remStud = new List<Student>();

                var stus = context.Students.Include(s => s.Group).Skip(start).Take(countArr).ToList();

                int GroupNumber, DepartmentNumber, CurrentCourse;

                foreach (var student in stus)
                {
                    GroupNumber = student.Group.GroupNumber;
                    DepartmentNumber = student.Group.DepartmentNumber;
                    CurrentCourse = student.Group.CurrentCourse;

                    var group = context.Groups.Where(o => o.GroupNumber == GroupNumber &&
                                                            o.DepartmentNumber == DepartmentNumber &&
                                                            o.CurrentCourse == CurrentCourse + 1).FirstOrDefault();

                    if (group == null) remStud.Add(student);
                    else student.GroupId = group.GroupId;

                }

                return true;

            }
            catch (Exception e)
            {

                Logger.LogError(e.Message);
                Logger.LogErrorTrace(e.StackTrace);
                
                return false;

            }

        }

        public static int FindOldestStudent(int start, int countArr)
        {

            ApplicationContext context = new ApplicationContext();
            int olderAge = getAge(context.Instructors.FirstOrDefault()?.DateOfBirth);

            var insts = context.Instructors.Skip(start).Take(countArr);
            foreach (var instructor in insts)
            {

                int age = getAge(instructor.DateOfBirth);

                if (age == -1) continue;
                if (age > olderAge || olderAge == -1)
                    olderAge = age;

            }

            return olderAge;

        }

        public static int FindYoungerInstructor(int start, int countArr)
        {

            ApplicationContext context = new ApplicationContext();
            int youngerAge = getAge(context.Students.FirstOrDefault()?.DateOfBirth);

            var stus = context.Students.Skip(start).Take(countArr);
            foreach (var student in stus)
            {

                int age = getAge(student.DateOfBirth);

                if(age == -1) continue;
                if (age < youngerAge || youngerAge == -1)
                    youngerAge = age;

            }

            return youngerAge;

        }

        private static int getAge(DateTime? DateOfBirth)
        {

            if (DateOfBirth == null) return -1;

            int age = DateTime.Today.Year - (DateOfBirth ?? default).Year;
            if ((DateOfBirth ?? default).Date > DateTime.Today.AddYears(-age)) age--;

            return age;

        }

    }
}
