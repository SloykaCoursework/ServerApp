using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ServerApp.Models
{
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DepartmentId { get; set; }  // Primary Key

        [Required, MaxLength(100)]
        public string DepartmentName { get; set; }

        public virtual ICollection<Instructor> Instructors { get; set; }
    }
}
