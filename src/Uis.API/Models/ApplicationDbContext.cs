using Microsoft.EntityFrameworkCore;
using Uis.API.Models;

namespace Uis.API.Models;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<CourseInstance> CourseInstances => Set<CourseInstance>();
    public DbSet<AcademicYear> AcademicYears => Set<AcademicYear>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<Grade> Grades => Set<Grade>();
    public DbSet<Announcement> Announcements => Set<Announcement>();
    public DbSet<Attendance> Attendances { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureUserEntity(modelBuilder);
        ConfigureDepartmentEntity(modelBuilder);
        ConfigureCourseEntity(modelBuilder);
        ConfigureCourseInstanceEntity(modelBuilder);
        ConfigureAcademicYearEntity(modelBuilder);
        ConfigureEnrollmentEntity(modelBuilder);
        ConfigureGradeEntity(modelBuilder);
        ConfigureAnnouncementEntity(modelBuilder);
        ConfigureAttendanceEntity(modelBuilder);
    }

    private void ConfigureUserEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<User>();

        entity.ToTable("Users");
        entity.HasKey(u => u.Id);

        entity.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255)
            .HasColumnName("Email");

        entity.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("FirstName");

        entity.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("LastName");

        entity.Property(u => u.PasswordHash)
            .IsRequired()
            .HasColumnName("PasswordHash");

        entity.Property(u => u.Role)
            .IsRequired()
            .HasConversion<string>()
            .HasColumnName("Role");

        entity.Property(u => u.CreatedAt)
            .IsRequired()
            .HasColumnType("datetime2")
            .HasDefaultValueSql("GETUTCDATE()")
            .HasColumnName("CreatedAt");

        entity.Property(u => u.UpdatedAt)
            .HasColumnType("datetime2")
            .HasColumnName("UpdatedAt");

        entity.HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName("IX_User_Email_Unique");

        entity.HasIndex(u => u.Role)
            .HasDatabaseName("IX_User_Role");

        entity.HasMany(u => u.StudentEnrollments)
            .WithOne(e => e.Student)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasMany(u => u.TaughtCourses)
            .WithOne(ci => ci.Teacher)
            .HasForeignKey(ci => ci.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasMany(u => u.GradesSubmitted)
            .WithOne(g => g.SubmittedByTeacher)
            .HasForeignKey(g => g.SubmittedByTeacherId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasMany(u => u.Announcements)
            .WithOne(a => a.CreatedByTeacher)
            .HasForeignKey(a => a.CreatedByTeacherId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private void ConfigureDepartmentEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Department>();

        entity.ToTable("Departments");
        entity.HasKey(d => d.Id);

        entity.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("Name");

        entity.Property(d => d.Code)
            .IsRequired()
            .HasMaxLength(10)
            .HasColumnName("Code");

        entity.Property(d => d.CreatedAt)
            .IsRequired()
            .HasColumnType("datetime2")
            .HasDefaultValueSql("GETUTCDATE()")
            .HasColumnName("CreatedAt");

        entity.Property(d => d.UpdatedAt)
            .HasColumnType("datetime2")
            .HasColumnName("UpdatedAt");

        entity.HasIndex(d => d.Code)
            .IsUnique()
            .HasDatabaseName("IX_Department_Code_Unique");

        entity.HasIndex(d => d.Name)
            .HasDatabaseName("IX_Department_Name");

        entity.HasMany(d => d.Courses)
            .WithOne(c => c.Department)
            .HasForeignKey(c => c.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private void ConfigureCourseEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Course>();

        entity.ToTable("Courses");
        entity.HasKey(c => c.Id);

        entity.Property(c => c.Code)
            .IsRequired()
            .HasMaxLength(10)
            .HasColumnName("Code");

        entity.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("Name");

        entity.Property(c => c.CreditHours)
            .IsRequired()
            .HasColumnName("CreditHours");

        entity.Property(c => c.DepartmentId)
            .IsRequired()
            .HasColumnName("DepartmentId");

        entity.Property(c => c.PrerequisiteCourseId)
            .HasColumnName("PrerequisiteCourseId");

        entity.Property(c => c.CreatedAt)
            .IsRequired()
            .HasColumnType("datetime2")
            .HasDefaultValueSql("GETUTCDATE()")
            .HasColumnName("CreatedAt");

        entity.Property(c => c.UpdatedAt)
            .HasColumnType("datetime2")
            .HasColumnName("UpdatedAt");

        entity.HasIndex(c => c.Code)
            .IsUnique()
            .HasDatabaseName("IX_Course_Code_Unique");

        entity.HasIndex(c => c.DepartmentId)
            .HasDatabaseName("IX_Course_DepartmentId");

        entity.HasOne(c => c.Department)
            .WithMany(d => d.Courses)
            .HasForeignKey(c => c.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        entity.HasOne(c => c.PrerequisiteCourse)
            .WithMany()
            .HasForeignKey(c => c.PrerequisiteCourseId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(false);

        entity.HasMany(c => c.Instances)
            .WithOne(ci => ci.Course)
            .HasForeignKey(ci => ci.CourseId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private void ConfigureCourseInstanceEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<CourseInstance>();

        entity.ToTable("CourseInstances");
        entity.HasKey(ci => ci.Id);

        entity.Property(ci => ci.CourseId)
            .IsRequired()
            .HasColumnName("CourseId");

        entity.Property(ci => ci.TeacherId)
            .IsRequired()
            .HasColumnName("TeacherId");

        entity.Property(ci => ci.AcademicYearId)
            .IsRequired()
            .HasColumnName("AcademicYearId");

        entity.Property(ci => ci.DepartmentId)
            .IsRequired()
            .HasColumnName("DepartmentId");

        entity.Property(ci => ci.Section)
            .IsRequired()
            .HasMaxLength(10)
            .HasColumnName("Section");

        entity.Property(ci => ci.Capacity)
            .IsRequired()
            .HasDefaultValue(30)
            .HasColumnName("Capacity");

        entity.Property(ci => ci.CurrentEnrollmentCount)
            .IsRequired()
            .HasDefaultValue(0)
            .HasColumnName("CurrentEnrollmentCount");

        entity.Property(ci => ci.Day1)
            .HasColumnName("Day1");

        entity.Property(ci => ci.Day2)
            .HasColumnName("Day2");

        entity.Property(ci => ci.StartTime)
            .HasColumnName("StartTime");

        entity.Property(ci => ci.EndTime)
            .HasColumnName("EndTime");

        entity.Property(ci => ci.Location)
            .HasMaxLength(200)
            .HasDefaultValue(string.Empty)
            .HasColumnName("Location");

        entity.Property(ci => ci.Status)
            .IsRequired()
            .HasConversion<int>()
            .HasColumnName("Status");

        entity.Property(ci => ci.ConcurrencyToken)
            .IsRowVersion()
            .HasColumnName("ConcurrencyToken");

        entity.Property(ci => ci.CreatedAt)
            .IsRequired()
            .HasColumnType("datetime2")
            .HasDefaultValueSql("GETUTCDATE()")
            .HasColumnName("CreatedAt");

        entity.Property(ci => ci.UpdatedAt)
            .HasColumnType("datetime2")
            .HasColumnName("UpdatedAt");

        entity.HasIndex(ci => ci.CourseId)
            .HasDatabaseName("IX_CourseInstance_CourseId");

        entity.HasIndex(ci => ci.TeacherId)
            .HasDatabaseName("IX_CourseInstance_TeacherId");

        entity.HasIndex(ci => ci.AcademicYearId)
            .HasDatabaseName("IX_CourseInstance_AcademicYearId");

        entity.HasIndex(ci => new { ci.AcademicYearId, ci.DepartmentId })
            .HasDatabaseName("IX_CourseInstance_AcademicYear_Department");

        entity.HasIndex(ci => new { ci.CourseId, ci.Section, ci.AcademicYearId })
            .IsUnique()
            .HasDatabaseName("IX_CourseInstance_Course_Section_Year_Unique");

        entity.HasIndex(ci => ci.Status)
            .HasFilter("[Status] = 1")
            .HasDatabaseName("IX_CourseInstance_Status_Active");

        entity.HasOne(ci => ci.Course)
            .WithMany(c => c.Instances)
            .HasForeignKey(ci => ci.CourseId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        entity.HasOne(ci => ci.Teacher)
            .WithMany(u => u.TaughtCourses)
            .HasForeignKey(ci => ci.TeacherId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        entity.HasOne(ci => ci.AcademicYear)
            .WithMany()
            .HasForeignKey(ci => ci.AcademicYearId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        entity.HasOne(ci => ci.Department)
            .WithMany()
            .HasForeignKey(ci => ci.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        entity.HasMany(ci => ci.Enrollments)
            .WithOne(e => e.CourseInstance)
            .HasForeignKey(e => e.CourseInstanceId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasMany(ci => ci.Grades)
            .WithOne(g => g.CourseInstance)
            .HasForeignKey(g => g.CourseInstanceId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private void ConfigureAcademicYearEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<AcademicYear>();

        entity.ToTable("AcademicYears");
        entity.HasKey(ay => ay.Id);

        entity.Property(ay => ay.Year)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnName("Year");

        entity.Property(ay => ay.StartYear)
            .IsRequired()
            .HasColumnName("StartYear");

        entity.Property(ay => ay.EndYear)
            .IsRequired()
            .HasColumnName("EndYear");

        entity.Property(ay => ay.EnrollmentStartDate)
            .IsRequired()
            .HasColumnType("datetime2")
            .HasColumnName("EnrollmentStartDate");

        entity.Property(ay => ay.EnrollmentEndDate)
            .IsRequired()
            .HasColumnType("datetime2")
            .HasColumnName("EnrollmentEndDate");

        entity.Property(ay => ay.IsActive)
            .IsRequired()
            .HasDefaultValue(false)
            .HasColumnName("IsActive");

        entity.Property(ay => ay.CreatedAt)
            .IsRequired()
            .HasColumnType("datetime2")
            .HasDefaultValueSql("GETUTCDATE()")
            .HasColumnName("CreatedAt");

        entity.Property(ay => ay.UpdatedAt)
            .HasColumnType("datetime2")
            .HasColumnName("UpdatedAt");

        entity.HasIndex(ay => ay.Year)
            .IsUnique()
            .HasDatabaseName("IX_AcademicYear_Year_Unique");

        entity.HasIndex(ay => ay.IsActive)
            .HasDatabaseName("IX_AcademicYear_IsActive");
    }

    private void ConfigureEnrollmentEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Enrollment>();

        entity.ToTable("Enrollments");
        entity.HasKey(e => e.Id);

        entity.Property(e => e.StudentId)
            .IsRequired()
            .HasColumnName("StudentId");

        entity.Property(e => e.CourseInstanceId)
            .IsRequired()
            .HasColumnName("CourseInstanceId");

        entity.Property(e => e.AcademicYearId)
            .IsRequired()
            .HasColumnName("AcademicYearId");

        entity.Property(e => e.Status)
            .IsRequired()
            .HasConversion<int>()
            .HasColumnName("Status");

        entity.Property(e => e.EnrolledAt)
            .IsRequired()
            .HasColumnType("datetime2")
            .HasColumnName("EnrolledAt");

        entity.Property(e => e.DroppedAt)
            .HasColumnType("datetime2")
            .HasColumnName("DroppedAt");

        entity.Property(e => e.CreatedAt)
            .IsRequired()
            .HasColumnType("datetime2")
            .HasDefaultValueSql("GETUTCDATE()")
            .HasColumnName("CreatedAt");

        entity.Property(e => e.UpdatedAt)
            .HasColumnType("datetime2")
            .HasColumnName("UpdatedAt");

        entity.HasIndex(e => new { e.StudentId, e.CourseInstanceId, e.AcademicYearId })
            .IsUnique()
            .HasDatabaseName("IX_Enrollment_Student_Course_Year_Unique");

        entity.HasIndex(e => new { e.CourseInstanceId, e.Status })
            .HasDatabaseName("IX_Enrollment_CourseInstance_Status");

        entity.HasIndex(e => new { e.StudentId, e.Status })
            .HasDatabaseName("IX_Enrollment_Student_Status");

        entity.HasIndex(e => e.AcademicYearId)
            .HasDatabaseName("IX_Enrollment_AcademicYearId");

        entity.HasOne(e => e.Student)
            .WithMany(u => u.StudentEnrollments)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        entity.HasOne(e => e.CourseInstance)
            .WithMany(ci => ci.Enrollments)
            .HasForeignKey(e => e.CourseInstanceId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        entity.HasOne(e => e.AcademicYear)
            .WithMany()
            .HasForeignKey(e => e.AcademicYearId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        entity.HasMany(e => e.Attendances)
            .WithOne(a => a.Enrollment)
            .HasForeignKey(a => a.EnrollmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private void ConfigureGradeEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Grade>();

        entity.ToTable("Grades");
        entity.HasKey(g => g.Id);

        entity.Property(g => g.StudentId)
            .IsRequired()
            .HasColumnName("StudentId");

        entity.Property(g => g.CourseInstanceId)
            .IsRequired()
            .HasColumnName("CourseInstanceId");

        entity.Property(g => g.SubmittedByTeacherId)
            .IsRequired()
            .HasColumnName("SubmittedByTeacherId");

        entity.Property(g => g.Score)
            .IsRequired()
            .HasPrecision(5, 2)
            .HasColumnName("Score");

        entity.Property(g => g.LetterGrade)
            .IsRequired()
            .HasMaxLength(2)
            .HasColumnName("LetterGrade");

        entity.Property(g => g.Notes)
            .HasMaxLength(1000)
            .HasDefaultValue(string.Empty)
            .HasColumnName("Notes");

        entity.Property(g => g.SubmittedAt)
            .HasColumnType("datetime2")
            .HasColumnName("SubmittedAt");

        entity.Property(g => g.CreatedAt)
            .IsRequired()
            .HasColumnType("datetime2")
            .HasDefaultValueSql("GETUTCDATE()")
            .HasColumnName("CreatedAt");

        entity.Property(g => g.UpdatedAt)
            .HasColumnType("datetime2")
            .HasColumnName("UpdatedAt");

        entity.HasIndex(g => new { g.StudentId, g.CourseInstanceId })
            .IsUnique()
            .HasDatabaseName("IX_Grade_Student_Course_Unique");

        entity.HasIndex(g => new { g.CourseInstanceId, g.SubmittedAt })
            .HasDatabaseName("IX_Grade_CourseInstance_SubmittedAt");

        entity.HasIndex(g => new { g.SubmittedByTeacherId, g.SubmittedAt })
            .HasDatabaseName("IX_Grade_SubmittedBy_SubmittedAt");

        entity.HasOne(g => g.Student)
            .WithMany()
            .HasForeignKey(g => g.StudentId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        entity.HasOne(g => g.CourseInstance)
            .WithMany(ci => ci.Grades)
            .HasForeignKey(g => g.CourseInstanceId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        entity.HasOne(g => g.SubmittedByTeacher)
            .WithMany(u => u.GradesSubmitted)
            .HasForeignKey(g => g.SubmittedByTeacherId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        entity.HasOne(g => g.AcademicYear)
            .WithMany()
            .IsRequired(false);
    }

    private void ConfigureAnnouncementEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Announcement>();

        entity.ToTable("Announcements");
        entity.HasKey(a => a.Id);

        entity.Property(a => a.CreatedByTeacherId)
            .IsRequired()
            .HasColumnName("CreatedByTeacherId");

        entity.Property(a => a.CreatedByTeacherName)
            .HasColumnName("CreatedByTeacherName");

        entity.Property(a => a.Title)
            .IsRequired()
            .HasMaxLength(300)
            .HasColumnName("Title");

        entity.Property(a => a.Content)
            .IsRequired()
            .HasMaxLength(5000)
            .HasColumnName("Content");

        entity.Property(a => a.PublishedAt)
            .IsRequired()
            .HasColumnType("datetime2")
            .HasColumnName("PublishedAt");

        entity.Property(a => a.TargetAcademicYearId)
            .HasColumnName("TargetAcademicYearId");

        entity.Property(a => a.TargetCourseInstanceId)
            .HasColumnName("TargetCourseInstanceId");

        entity.Property(a => a.CreatedAt)
            .IsRequired()
            .HasColumnType("datetime2")
            .HasDefaultValueSql("GETUTCDATE()")
            .HasColumnName("CreatedAt");

        entity.Property(a => a.UpdatedAt)
            .HasColumnType("datetime2")
            .HasColumnName("UpdatedAt");

        entity.HasIndex(a => a.CreatedByTeacherId)
            .HasDatabaseName("CreatedByTeacherId");

        entity.HasOne(a => a.CreatedByTeacher)
            .WithMany(u => u.Announcements)
            .HasForeignKey(a => a.CreatedByTeacherId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        entity.HasOne(a => a.TargetAcademicYear)
            .WithMany()
            .HasForeignKey(a => a.TargetAcademicYearId)
            .OnDelete(DeleteBehavior.SetNull);

        entity.HasOne(a => a.TargetCourseInstance)
            .WithMany()
            .HasForeignKey(a => a.TargetCourseInstanceId)
            .OnDelete(DeleteBehavior.SetNull);

        entity.HasOne(a => a.TargetDepartment)
            .WithMany()
            .IsRequired(false);
    }

    private void ConfigureAttendanceEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Attendance>();

        entity.ToTable("Attendances");
        entity.HasKey(a => a.Id);

        entity.Property(a => a.EnrollmentId)
            .IsRequired()
            .HasColumnName("EnrollmentId");

        entity.Property(a => a.Week)
            .IsRequired()
            .HasColumnName("Week");

        entity.Property(a => a.Status)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnName("Status");

        entity.Property(a => a.CreatedAt)
            .IsRequired()
            .HasColumnType("datetime2")
            .HasDefaultValueSql("GETUTCDATE()")
            .HasColumnName("CreatedAt");

        entity.Property(a => a.UpdatedAt)
            .HasColumnType("datetime2")
            .HasColumnName("UpdatedAt");

        entity.HasIndex(a => new { a.EnrollmentId, a.Week, a.Day })
            .IsUnique()
            .HasDatabaseName("IX_Attendance_Enrollment_Week_Day_Unique");

        entity.HasIndex(a => a.Week)
            .HasDatabaseName("IX_Attendance_Week");

        entity.HasOne(a => a.Enrollment)
            .WithMany(e => e.Attendances)
            .HasForeignKey(a => a.EnrollmentId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}