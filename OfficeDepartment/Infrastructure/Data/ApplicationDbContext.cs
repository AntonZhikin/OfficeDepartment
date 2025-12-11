using Microsoft.EntityFrameworkCore;
using OfficeDepartment.Domain.Entities;

namespace OfficeDepartment.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<HeadOffice> HeadOffices { get; init; }
    public DbSet<BranchOffice> BranchOffices { get; init; }
    public DbSet<OfficeTask> OfficeTasks { get; init; }
    public DbSet<Employee> Employees { get; init; }
    public DbSet<Department> Departments { get; init; }
    public DbSet<User> Users { get; init; }
    public DbSet<AuditLog> AuditLogs { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // HeadOffice configuration
        modelBuilder.Entity<HeadOffice>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Address).IsRequired().HasMaxLength(500);
            entity.Property(e => e.City).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Country).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
        });

        // BranchOffice configuration
        modelBuilder.Entity<BranchOffice>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Address).IsRequired().HasMaxLength(500);
            entity.Property(e => e.City).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Country).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
            
            entity.HasOne(e => e.HeadOffice)
                .WithMany(h => h.BranchOffices)
                .HasForeignKey(e => e.HeadOfficeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // OfficeTask configuration
        modelBuilder.Entity<OfficeTask>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(2000);
            
            entity.HasOne(e => e.BranchOffice)
                .WithMany(b => b.Tasks)
                .HasForeignKey(e => e.BranchOfficeId)
                .OnDelete(DeleteBehavior.SetNull);
            
            entity.HasOne(e => e.AssignedEmployee)
                .WithMany(emp => emp.AssignedTasks)
                .HasForeignKey(e => e.AssignedEmployeeId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Employee configuration
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
            entity.Property(e => e.Position).IsRequired().HasMaxLength(100);
            
            entity.HasOne(e => e.BranchOffice)
                .WithMany(b => b.Employees)
                .HasForeignKey(e => e.BranchOfficeId)
                .OnDelete(DeleteBehavior.SetNull);
            
            entity.HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Department configuration
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            
            entity.HasOne(e => e.HeadOffice)
                .WithMany(h => h.Departments)
                .HasForeignKey(e => e.HeadOfficeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
            entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Role).IsRequired().HasMaxLength(50);
            
            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // AuditLog configuration
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Action).IsRequired().HasMaxLength(50);
            entity.Property(e => e.EntityType).IsRequired().HasMaxLength(100);
        });
    }
}

