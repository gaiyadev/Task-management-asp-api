﻿using Microsoft.EntityFrameworkCore;
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
        
        // Configure the one-to-many relationship
        modelBuilder.Entity<Todo>()
            .HasOne(t => t.User)         // Todo has one User
            .WithMany(u => u.Todos)      // User has many Todos
            .HasForeignKey(t => t.UserId); // Define the foreign key

    }
}