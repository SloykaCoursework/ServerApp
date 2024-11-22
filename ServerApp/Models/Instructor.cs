using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace ServerApp.Models
{
    public class Instructor
    {
        public int InstructorId { get; set; }  // Primary Key

        [Required, MaxLength(100)]
        public string FirstName { get; set; }

        [Required, MaxLength(100)]
        public string LastName { get; set; }

        public int DepartmentId { get; set; }  // Foreign Key
        public virtual Department Department { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}
