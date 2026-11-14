using EducationCenter.Models;
using Microsoft.EntityFrameworkCore;

namespace EducationCenter.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);
            b.Entity<Course>().Property(c => c.Price).HasPrecision(10, 2);
            b.Entity<Enrollment>().HasIndex(e => new { e.CourseId, e.StudentId }).IsUnique();
        }
    }
}
