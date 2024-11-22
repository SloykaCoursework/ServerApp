namespace ServerApp.Models
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }  // Primary Key

        public int CourseId { get; set; }      // Foreign Key
        public virtual Course Course { get; set; }

        public int StudentId { get; set; }     // Foreign Key
        public virtual Student Student { get; set; }

        public Grade? Grade { get; set; }      // Может быть пустым
    }
}