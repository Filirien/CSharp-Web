namespace StudentSystem
{
    using Microsoft.EntityFrameworkCore;
    using StudentSystem.Data;
    using System;

    public class StudentDbContext : DbContext
    {

        public DbSet<Course> Courses { get; set; }

        public DbSet<Homework> Homeworks { get; set; }

        public DbSet<Resource> Resources { get; set; }

        public DbSet<Student> Students { get; set; }
        

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=StudentSystem;Integrated Security=True;");
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<StudentsCourses>()
                .HasKey(sc => new { sc.StudentId, sc.CourseId });

            builder
                .Entity<Course>()
                .HasMany(c => c.Students)
                .WithOne(s => s.Course)
                .HasForeignKey(c => c.CourseId);

            builder
                .Entity<Student>()
                .HasMany(s => s.Courses)
                .WithOne(c => c.Student)
                .HasForeignKey(s => s.StudentId);

            builder
                .Entity<Course>()
                .HasMany(c => c.Resources)
                .WithOne(r => r.Course)
                .HasForeignKey(r => r.CourseId);

            builder
                .Entity<Homework>()
                .HasOne(h => h.Student)
                .WithMany(s => s.HomeworkSubmissions)
                .HasForeignKey(h => h.StudentId);

            builder
                .Entity<Homework>()
                .HasOne(s => s.Student)
                .WithMany(h => h.HomeworkSubmissions)
                .HasForeignKey(s => s.StudentId);
        }
    }
}
