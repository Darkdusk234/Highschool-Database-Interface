using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Highschool_Database_Interface.Models;

public partial class HighschoolContext : DbContext
{
    public HighschoolContext()
    {
    }

    public HighschoolContext(DbContextOptions<HighschoolContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeRole> EmployeeRoles { get; set; }

    public virtual DbSet<Grade> Grades { get; set; }

    public virtual DbSet<PossibleGrade> PossibleGrades { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=localhost;Database=Highschool;Integrated Security=True;Trust Server Certificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.ClassId).HasName("PK__classes__FDF4798633EDB8F3");

            entity.ToTable("classes");

            entity.Property(e => e.ClassId).HasColumnName("class_id");
            entity.Property(e => e.ClassName)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("class_name");
            entity.Property(e => e.TeacherId).HasColumnName("teacher_id");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Classes)
                .HasForeignKey(d => d.TeacherId)
                .HasConstraintName("FK__classes__teacher__3C69FB99");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__courses__8F1EF7AEFA484A31");

            entity.ToTable("courses");

            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.CourseActive).HasColumnName("course_active");
            entity.Property(e => e.CourseName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("course_name");
            entity.Property(e => e.DepartmentId).HasColumnName("department_id");

            entity.HasOne(d => d.Department).WithMany(p => p.Courses)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK__courses__departm__52593CB8");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK__departme__C2232422CB45DF61");

            entity.ToTable("departments");

            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("department_name");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__employee__C52E0BA83D161252");

            entity.ToTable("employees");

            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.DateStarted)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("dateStarted");
            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("last_name");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Salary)
                .HasColumnType("decimal(20, 2)")
                .HasColumnName("salary");

            entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("fk_department_id");

            entity.HasOne(d => d.Role).WithMany(p => p.Employees)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__employees__role___398D8EEE");
        });

        modelBuilder.Entity<EmployeeRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__employee__760965CC72BF5363");

            entity.ToTable("employee_roles");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<Grade>(entity =>
        {
            entity.HasKey(e => e.GradeId).HasName("PK__grades__3A8F732C739E46B1");

            entity.ToTable("grades");

            entity.Property(e => e.GradeId).HasColumnName("grade_id");
            entity.Property(e => e.GradeSet)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("grade_set");
            entity.Property(e => e.StudentGradeId).HasColumnName("student_grade_id");
            entity.Property(e => e.StudentId).HasColumnName("student_id");
            entity.Property(e => e.SubjectId).HasColumnName("subject_id");
            entity.Property(e => e.TeacherId).HasColumnName("teacher_id");

            entity.HasOne(d => d.StudentGrade).WithMany(p => p.Grades)
                .HasForeignKey(d => d.StudentGradeId)
                .HasConstraintName("fk_student_grade_id");

            entity.HasOne(d => d.Student).WithMany(p => p.Grades)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__grades__student___4222D4EF");

            entity.HasOne(d => d.Subject).WithMany(p => p.Grades)
                .HasForeignKey(d => d.SubjectId)
                .HasConstraintName("fk_subject_id");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Grades)
                .HasForeignKey(d => d.TeacherId)
                .HasConstraintName("FK__grades__teacher___4316F928");
        });

        modelBuilder.Entity<PossibleGrade>(entity =>
        {
            entity.HasKey(e => e.GradeId).HasName("PK__possible__3A8F732CE59FC63E");

            entity.ToTable("possible_grades");

            entity.Property(e => e.GradeId).HasColumnName("grade_id");
            entity.Property(e => e.GradeLetter)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("grade_letter");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__students__2A33069A32F0FBD6");

            entity.ToTable("students");

            entity.Property(e => e.StudentId).HasColumnName("student_id");
            entity.Property(e => e.ClassId).HasColumnName("class_id");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("last_name");
            entity.Property(e => e.SocialSecurity)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("social_security");

            entity.HasOne(d => d.Class).WithMany(p => p.Students)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("FK__students__class___3F466844");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.SubjectId).HasName("PK__subjects__5004F660A222D2EB");

            entity.ToTable("subjects");

            entity.Property(e => e.SubjectId).HasColumnName("subject_id");
            entity.Property(e => e.SubjectCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("subject_code");
            entity.Property(e => e.SubjectName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("subject_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
