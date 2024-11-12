using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServerApp.Models;

public class Student
{

    public int StudentId { get; set; }  // Primary Key

    [Required, MaxLength(100)]
    public string FirstName { get; set; }

    [Required, MaxLength(100)]
    public string LastName { get; set; }
    
    [Required]
    public DateTime? DateOfBirth { get; set; }  // Может быть пустой

    public int GroupId { get; set; }  // Foreign Key
    public virtual Group Group { get; set; }

    public virtual ICollection<Enrollment> Enrollments { get; set; }

}
