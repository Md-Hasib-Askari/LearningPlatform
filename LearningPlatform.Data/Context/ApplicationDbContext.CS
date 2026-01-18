using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Course> Courses { get; set; } = null!;
    public DbSet<Module> Modules { get; set; } = null!;
    public DbSet<Lesson> Lessons { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Course>()
            .HasOne(c => c.Instructor)
            .WithMany()
            .HasForeignKey(c => c.InstructorId)
            .OnDelete(DeleteBehavior.Cascade);

        // modelBuilder.Entity<Module>()
        //     .HasOne(m => m.Course)
        //     .WithMany(c => c.Modules)
        //     .HasForeignKey(m => m.CourseId);

        // modelBuilder.Entity<Lesson>()
        //     .HasOne(l => l.Module)
        //     .WithMany(m => m.Lessons)
        //     .HasForeignKey(l => l.ModuleId);
    }
}