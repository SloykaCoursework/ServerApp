using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerApp.Models
{
    public class Enrollment
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EnrollmentId { get; set; }  // Primary Key

        public int CourseId { get; set; }      // Foreign Key
        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }

        public int StudentId { get; set; }     // Foreign Key
        [ForeignKey("StudentId")]
        public Student Student { get; set; }

        public Grade? Grade { get; set; }      // Может быть пустым
    }
}