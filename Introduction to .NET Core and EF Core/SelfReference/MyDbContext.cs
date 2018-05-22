namespace ManyToMany
{
    using Microsoft.EntityFrameworkCore;
    public class MyDbContext : DbContext
    {
        DbSet<Student> Student { get; set; }
        DbSet<Course> Course { get; set; }
        DbSet<StudentsCourses> StudentsCourses { get;set;}
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=ManyToMany;Integrated Security=True;");

            base.OnConfiguring(builder);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<StudentsCourses>()
                .HasKey(sc => new { sc.StudentId, sc.CourseId });


            builder.Entity<StudentsCourses>()
                .HasOne(sc => sc.Student)
                .WithMany(st => st.StudentsCourses)
                .HasForeignKey(sc => sc.StudentId);

            builder.Entity<StudentsCourses>()
              .HasOne(sc => sc.Course)
              .WithMany(st => st.CoursesStudents)
              .HasForeignKey(sc => sc.CourseId);

           
        }
    }

}