using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ZulfieP.Models.Entities
{
    public partial class SalariesContext : DbContext
    {
        public SalariesContext()
        {
        }

        public SalariesContext(DbContextOptions<SalariesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<Grades> Grades { get; set; }
        public virtual DbSet<Passwords> Passwords { get; set; }
        public virtual DbSet<Salaries> Salaries { get; set; }
        public virtual DbSet<ScientificTitles> ScientificTitles { get; set; }
        public virtual DbSet<Stages> Stages { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=Salaries;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employees>(entity =>
            {
                entity.ToTable("employees");

                entity.Property(e => e.Id).HasColumnName("id").UseIdentityColumn();

                entity.Property(e => e.Birthdate)
                    .HasColumnName("birthdate")
                    .HasColumnType("date");

                entity.Property(e => e.DepartmentId).HasColumnName("department_id");

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasColumnName("full_name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.GradeId).HasColumnName("grade_id");

                entity.Property(e => e.IdentityNumber)
                    .IsRequired()
                    .HasColumnName("identity_number")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.IsRatired).HasColumnName("is_ratired");

                entity.Property(e => e.KidsNumber).HasColumnName("kids_number");

                entity.Property(e => e.MarrigeStatus).HasColumnName("marrige_status");

                entity.Property(e => e.Section).HasColumnName("section");

                entity.Property(e => e.StageId).HasColumnName("stage_id");

                entity.HasOne(d => d.Grade)
                    .WithMany(p => p.EmployeesGrade)
                    .HasForeignKey(d => d.GradeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_employees_grades");

                entity.HasOne(d => d.Stage)
                    .WithMany(p => p.EmployeesStage)
                    .HasForeignKey(d => d.StageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_employees_stages");
            });

            modelBuilder.Entity<Grades>(entity =>
            {
                entity.ToTable("grades");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityColumn();

                entity.Property(e => e.GradeName)
                    .IsRequired()
                    .HasColumnName("grade_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            });

            modelBuilder.Entity<Passwords>(entity =>
            {
                entity.ToTable("passwords");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityColumn();

                entity.Property(e => e.HashedPassword)
                    .IsRequired()
                    .HasColumnName("hashed_password")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordSalt)
                    .IsRequired()
                    .HasColumnName("password_salt")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Salaries>(entity =>
            {
                entity.ToTable("salaries");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityColumn();

                entity.Property(e => e.DegreeAllotments).HasColumnName("degree_allotments");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("text");

                entity.Property(e => e.EmployeeId).HasColumnName("employee_id");

                entity.Property(e => e.IncomeTax).HasColumnName("income_tax");

                entity.Property(e => e.InitialSalary).HasColumnName("initial_salary");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.KidsAllotments).HasColumnName("kids_allotments");

                entity.Property(e => e.MarrigeAllotments).HasColumnName("marrige_allotments");

                entity.Property(e => e.OtherSubtractions).HasColumnName("other_subtractions");

                entity.Property(e => e.PositionAllotments).HasColumnName("position_allotments");

                entity.Property(e => e.RetirementSubtraction).HasColumnName("retirement_subtraction");

                entity.Property(e => e.ScientificTitleId).HasColumnName("scientific_title");

                entity.Property(e => e.TransportationAllotments).HasColumnName("transportation_allotments");

                entity.Property(e => e.UniAllotments).HasColumnName("uni_allotments");

                entity.Property(e => e.VacationDiff).HasColumnName("total_amount");

                entity.Property(e => e.VacationDiff).HasColumnName("vacation_diff");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Salaries)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_salaries_employees");
                entity.HasOne(d => d.ScientificTitles)
                   .WithMany(p => p.Salaries)
                   .HasForeignKey(d => d.ScientificTitleId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_salaries_scientific_titles");
            });

            modelBuilder.Entity<ScientificTitles>(entity =>
            {
                entity.ToTable("scientific_titles");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityColumn();

                entity.Property(e => e.Income).HasColumnName("income");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ScientificTitle)
                    .IsRequired()
                    .HasColumnName("scientific_title")
                    .HasMaxLength(255)
                    .IsUnicode(false);


            });

            modelBuilder.Entity<Stages>(entity =>
            {
                entity.ToTable("stages");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityColumn();

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.StageName)
                    .IsRequired()
                    .HasColumnName("stage_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityColumn();

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasColumnName("full_name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive)
                    .HasColumnName("is_active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.PasswordId).HasColumnName("password_id");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Password)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.PasswordId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_users_passwords");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
