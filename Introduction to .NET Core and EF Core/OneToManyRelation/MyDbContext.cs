namespace OneToManyRelation
{
    using Microsoft.EntityFrameworkCore;
    public class MyDbContext : DbContext
    {
        DbSet<Employee> Employee { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=MyTestDb;Integrated Security=True;");

            base.OnConfiguring(builder);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(emp => emp.DepartmentId);

            builder.Entity<Employee>()
                .HasOne(emp => emp.Manager)
                .WithMany(m => m.Employees)
                .HasForeignKey(emp => emp.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}