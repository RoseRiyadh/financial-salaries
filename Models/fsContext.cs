using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ZulfieP.Models
{
    public partial class fsContext : DbContext
    {
        public fsContext()
        {
        }

        public fsContext(DbContextOptions<fsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<College> College { get; set; }
        public virtual DbSet<Degree> Degree { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<Grade> Grade { get; set; }
        public virtual DbSet<JobTitle> JobTitle { get; set; }
        public virtual DbSet<MarrigeStatus> MarrigeStatus { get; set; }
        public virtual DbSet<Position> Position { get; set; }
        public virtual DbSet<Room> Room { get; set; }
        public virtual DbSet<Salary> Salary { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=fs;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<College>(entity =>
            {
                entity.ToTable("college");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever()
                    .UseIdentityColumn();

                entity.Property(e => e.Income)
                    .HasColumnName("income")
                    .HasColumnType("numeric(8, 0)");

                entity.Property(e => e.Major)
                    .HasColumnName("major")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Degree>(entity =>
            {
                entity.ToTable("degree");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever()
                    .UseIdentityColumn();

                entity.Property(e => e.Income)
                .IsRequired()
                .HasColumnName("income")
                    .HasColumnType("numeric(8, 0)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("employee");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever()
                    .UseIdentityColumn();

                entity.Property(e => e.Birthdate)
                    .HasColumnName("birthdate")
                    .HasColumnType("date");

                entity.Property(e => e.CollegeId).HasColumnName("college_id");

                entity.Property(e => e.DegreeId).HasColumnName("degree_id");

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasColumnName("full_name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.GradeId).HasColumnName("grade_id");

                entity.Property(e => e.JobTitleId).HasColumnName("job_title_id");

                entity.Property(e => e.KidsNumber).HasColumnName("kids_number");

                entity.Property(e => e.MarrigeStatusId).HasColumnName("marrige_status_id");

                entity.Property(e => e.PositionId).HasColumnName("position_id");

                entity.Property(e => e.RoomId).HasColumnName("room_id");

                entity.Property(e => e.StartingDate)
                    .HasColumnName("starting_date")
                    .HasColumnType("smalldatetime");

                entity.HasOne(d => d.College)
                    .WithMany(p => p.Employee)
                    .HasForeignKey(d => d.CollegeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_employee_college");

                entity.HasOne(d => d.Degree)
                    .WithMany(p => p.Employee)
                    .HasForeignKey(d => d.DegreeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_employee_degree");

                entity.HasOne(d => d.Grade)
                    .WithMany(p => p.Employee)
                    .HasForeignKey(d => d.GradeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_employee_grade");

                entity.HasOne(d => d.JobTitle)
                    .WithMany(p => p.Employee)
                    .HasForeignKey(d => d.JobTitleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_employee_job_title");

                entity.HasOne(d => d.MarrigeStatus)
                    .WithMany(p => p.Employee)
                    .HasForeignKey(d => d.MarrigeStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_employee_marrige_status");

                entity.HasOne(d => d.Position)
                    .WithMany(p => p.Employee)
                    .HasForeignKey(d => d.PositionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_employee_position1");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Employee)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_employee_room");
            });

            modelBuilder.Entity<Grade>(entity =>
            {
                entity.ToTable("grade");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever()
                    .UseIdentityColumn();

                entity.Property(e => e.Income)
                    .HasColumnName("income")
                    .HasColumnType("numeric(8, 0)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<JobTitle>(entity =>
            {
                entity.ToTable("job_title");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever()
                    .UseIdentityColumn();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MarrigeStatus>(entity =>
            {
                entity.ToTable("marrige_status");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever()
                    .UseIdentityColumn();

                entity.Property(e => e.Income)
                    .HasColumnName("income")
                    .HasColumnType("numeric(8, 0)");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.ToTable("position");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever()
                    .UseIdentityColumn();

                entity.Property(e => e.Income)
                    .HasColumnName("income")
                    .HasColumnType("numeric(8, 0)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

               
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.ToTable("room");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever()
                    .UseIdentityColumn();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Salary>(entity =>
            {
                entity.ToTable("salary");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever()
                    .UseIdentityColumn();

                entity.Property(e => e.AnotherAllotments)
                    .HasColumnName("another_allotments")
                    .HasColumnType("numeric(8, 0)");

                entity.Property(e => e.CollegeAllotments)
                    .HasColumnName("college_allotments")
                    .HasColumnType("numeric(8, 0)");

                entity.Property(e => e.DegreeAllotments)
                    .HasColumnName("degree_allotments")
                    .HasColumnType("numeric(8, 0)");

                entity.Property(e => e.EmployeeId).HasColumnName("employee_id");

                entity.Property(e => e.IncomeTax)
                    .HasColumnName("income_tax")
                    .HasColumnType("numeric(8, 0)");

                entity.Property(e => e.InitialSalary)
                    .HasColumnName("initial_salary")
                    .HasColumnType("numeric(8, 0)");

                entity.Property(e => e.KidAllotments)
                    .HasColumnName("kid_allotments")
                    .HasColumnType("numeric(8, 0)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.MarrigeAllotments)
                    .HasColumnName("marrige_allotments")
                    .HasColumnType("numeric(8, 0)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PositionAllotments)
                    .HasColumnName("position_allotments")
                    .HasColumnType("numeric(8, 0)");

                entity.Property(e => e.RetirementAllotments)
                    .HasColumnName("retirement_allotments")
                    .HasColumnType("numeric(8, 0)");

                entity.Property(e => e.TotalAmount)
                    .HasColumnName("total_amount")
                    .HasColumnType("numeric(8, 0)");

                entity.Property(e => e.TransportationAllotments)
                    .HasColumnName("transportation_allotments")
                    .HasColumnType("numeric(8, 0)");

                entity.Property(e => e.VacationDistinction)
                    .HasColumnName("vacation_distinction")
                    .HasColumnType("numeric(8, 0)");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Salary)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_salary_employee");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever()
                    .UseIdentityColumn();

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasColumnName("full_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Permission).HasColumnName("permission");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
