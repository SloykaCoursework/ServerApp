using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace ServerApp.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }  // Primary Key

        [Required, MaxLength(100)]
        public string DepartmentName { get; set; }

        public virtual ICollection<Instructor> Instructors { get; set; }
    }
}
