using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ServerApp.Models
{
    public class Instructor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InstructorId { get; set; }  // Primary Key

        [Required, MaxLength(100)]
        public string FirstName { get; set; }

        [Required, MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        public DateTime? DateOfBirth { get; set; }  // Может быть пустой

        public int DepartmentId { get; set; }  // Foreign Key
        public virtual Department Department { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}
