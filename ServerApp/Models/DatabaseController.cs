using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Models
{
    internal static class DatabaseController
    {

        static DatabaseController()
        {

            new ApplicationContext().SaveChanges();

        }

        public static bool CreateData(int count)
        {

            var context = new ApplicationContext();

            try
            {

                for (int i = 0; i < count; i++)
                {

                    context.Students.Add(new Student()
                    {

                        FirstName = Faker.Name.First(),
                        LastName = Faker.Name.Last(),
                        

                    });

                }

                context.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {

                Logger.LogError(ex.Message);
                Logger.LogErrorTrace(ex.StackTrace!);

                return false;

            }

        }

        public static int FindArithmeticMeanValues()
        {



        }

        public static int CountAllGroups()
        {



        }

        public static bool ChangeCourseToAll()
        {



        }

        public static int FindOldestStudent()
        {



        }

        public static int FindYoungerInstructor()
        {



        }

    }
}
