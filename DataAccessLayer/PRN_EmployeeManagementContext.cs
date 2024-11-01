using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models; 
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DataAccessLayer
{
    public partial class PRN_EmployeeManagementContext : DbContext
    {
        public PRN_EmployeeManagementContext()
        {
        }

        public PRN_EmployeeManagementContext(DbContextOptions<PRN_EmployeeManagementContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Departments> Departments { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<Salaries> Salaries { get; set; }
        public virtual DbSet<Attendances> Attendance { get; set; }
        public virtual DbSet<LeaveBalances> LeaveBalances { get; set; }
        public virtual DbSet<Notifications> Notifications { get; set; }
        public virtual DbSet<ActivityLogs> ActivityLogs { get; set; }

        public virtual DbSet<SalaryModification> SalaryModifications { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.EnableDetailedErrors()
                         .EnableSensitiveDataLogging(); // Chỉ dùng trong development

            {
                var ConnectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("EmployeeManagementDB");
                optionsBuilder.UseSqlServer(ConnectionString);
            }
        }

        // Method to retrieve connection string from appsettings.json
        //private string GetConnectionString()
        //{
        //    IConfiguration configuration = new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("appsettings.json")
        //        .Build();

        //    return configuration["ConnectionStrings:EmployeeManagementDB"];
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Role
            modelBuilder.Entity<Roles>(entity =>
            {
                entity.HasKey(e => e.RoleID);
                entity.Property(e => e.RoleName).HasMaxLength(50).IsRequired();
            });

            // Configure User
            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserID);
                entity.Property(e => e.Username).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Password).HasMaxLength(255).IsRequired();
                entity.HasOne(d => d.Roles)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Department
            modelBuilder.Entity<Departments>(entity =>
            {
                entity.HasKey(e => e.DepartmentID);
                entity.Property(e => e.DepartmentName).HasMaxLength(100).IsRequired();
            });

            // Configure Employee
            modelBuilder.Entity<Employees>(entity =>
            {
                entity.HasKey(e => e.EmployeeID);
                entity.Property(e => e.FullName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.BirthDate).HasColumnType("date");
                entity.HasOne(d => d.Users)
                    .WithOne(p => p.Employees)
                    .HasForeignKey<Employees>(d => d.UserID)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(d => d.Departments)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.DepartmentID);
            });

            // Configure Salary
            modelBuilder.Entity<Salaries>(entity =>
            {
                entity.HasKey(e => e.SalaryID);
                entity.Property(e => e.Month).IsRequired();
                entity.Property(e => e.Year).IsRequired();
                entity.HasOne(d => d.Employees)
                    .WithMany(p => p.Salaries)
                    .HasForeignKey(d => d.EmployeeID)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            //

            modelBuilder.Entity<SalaryModification>(entity =>
            {
                entity.ToTable("SalaryModification");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Amount).HasColumnName("amount");
                entity.Property(e => e.Date).HasColumnName("date");
                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsFixedLength()
                    .HasColumnName("description");
                entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .IsFixedLength()
                    .HasColumnName("status");
            });
            //

            // Configure Attendance
            modelBuilder.Entity<Attendances>(entity =>
            {
                entity.HasKey(e => e.AttendanceID);
                entity.Property(e => e.Date).HasColumnType("date").IsRequired();
                entity.HasOne(d => d.Employees)
                    .WithMany(p => p.Attendances)
                    .HasForeignKey(d => d.EmployeeID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure LeaveBalance
            modelBuilder.Entity<LeaveBalances>(entity =>
            {
                entity.HasKey(e => e.LeaveID);
                entity.Property(e => e.AnnualLeave).HasDefaultValue(12);
                entity.Property(e => e.SickLeave).HasDefaultValue(30);
                entity.HasOne(d => d.Employees)
                    .WithMany(p => p.LeaveBalances)
                    .HasForeignKey(d => d.EmployeeID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Notification
            modelBuilder.Entity<Notifications>(entity =>
            {
                entity.HasKey(e => e.NotificationID);
                entity.Property(e => e.Title).HasMaxLength(100).IsRequired();
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");
                entity.HasOne(d => d.Departments)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.DepartmentID);
            });

            // Configure ActivityLog
            modelBuilder.Entity<ActivityLogs>(entity =>
            {
                entity.HasKey(e => e.LogID);
                entity.Property(e => e.Action).HasMaxLength(100);
                entity.Property(e => e.LogDate).HasDefaultValueSql("GETDATE()");
                entity.HasOne(d => d.Users)
                    .WithMany(p => p.ActivityLogs)
                    .HasForeignKey(d => d.UserID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
