using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ServerApp.Models
{
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CourseId { get; set; }  // Primary Key

        [Required, MaxLength(100)]
        public string CourseName { get; set; }

        public int InstructorId { get; set; }  // Foreign Key
        public virtual Instructor Instructor { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
