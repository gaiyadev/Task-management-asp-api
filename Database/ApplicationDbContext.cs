using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Database;

public class ApplicationDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    public required DbSet<User> Users { get; set; }
    public required DbSet<Todo> Todos { get; set; }
    
    public required DbSet<Profile> Profiles { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(e => e.Email)
            .IsUnique();
        base.OnModelCreating(modelBuilder);
        
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .Property(e => e.IsActive)
            .HasDefaultValue(false);
        
        modelBuilder.Entity<User>()
            .Property(e => e.ResetToken)
            .IsRequired(false);

        // modelBuilder.Entity<User>()
        //     .HasOne(user => user.Profile) // User has one Profile
        //     .WithOne(profile => profile.User) // Profile has one User
        //     .HasForeignKey<Profile>(profile => profile.UserId).IsRequired(); // Profile.UserId is the foreign key
        // // Relationship is required (one-to-one)

    }
}