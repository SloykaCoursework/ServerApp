using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ServerApp.Models;

public class ApplicationContext : DbContext
{

    public DbSet<Student> Students { get; set; }
    public DbSet<Instructor> Instructors { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Group> Groups { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>()
            .Property(s => s.StudentId)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Instructor>()
            .Property(i => i.InstructorId)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Course>()
            .Property(c => c.CourseId)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Enrollment>()
            .Property(e => e.EnrollmentId)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Department>()
            .Property(d => d.DepartmentId)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Group>()
            .Property(g => g.GroupId)
            .ValueGeneratedOnAdd();
    }

    public ApplicationContext()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=127.0.0.1;Database=coursework;Trusted_Connection=False;User Id=sa;Password=zdeL-aL8pw;TrustServerCertificate=True");
    }

}