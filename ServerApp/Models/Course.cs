using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace ServerApp.Models
{
    public class Course
    {
        public int CourseId { get; set; }  // Primary Key

        [Required, MaxLength(100)]
        public string CourseName { get; set; }

        public int Credits { get; set; }

        public int InstructorId { get; set; }  // Foreign Key
        public virtual Instructor Instructor { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
