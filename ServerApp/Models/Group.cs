using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ServerApp.Models
{
    public class Group
    {
        public int GroupId { get; set; }  // Primary Key

        [NotMapped]
        public string GroupName { get { return $"{DepartmentNumber}{CurrentCourse}{GroupNumber:D2}"; } }

        [Required, Range(1, 6)]
        public int CurrentCourse { get; set; }  // Текущий курс студента

        [Required, Range(1, 99)]
        public int GroupNumber { get; set; }  // Номер группы (2-значное число)

        [Required, Range(1, 9)]
        public int DepartmentNumber { get; set; }  // Номер подразделения (от 1 до 9)

        public virtual ICollection<Student> Students { get; set; }
    }
}
