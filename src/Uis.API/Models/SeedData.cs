using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uis.API.Constants;
using Uis.API.Models;

namespace Uis.API.Data;

public static class SeedData
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // Check if data already exists
        if (context.Users.Any())
        {
            return;
        }

        // ==================== ACADEMIC YEAR ====================
        var academicYear = new AcademicYear
        {
            StartYear = 2024,
            EndYear = 2025,
            Year = "2024-2025",
            EnrollmentStartDate = DateTime.UtcNow.AddDays(-30),
            EnrollmentEndDate = DateTime.UtcNow.AddDays(60),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        context.AcademicYears.Add(academicYear);

        // ==================== DEPARTMENTS ====================
        var departments = new List<Department>
        {
            new Department { Code = "CSE", Name = "Computer Science & Engineering", CreatedAt = DateTime.UtcNow },
            new Department { Code = "ENG", Name = "Electrical Engineering", CreatedAt = DateTime.UtcNow },
            new Department { Code = "MATH", Name = "Mathematics", CreatedAt = DateTime.UtcNow },
            new Department { Code = "PHYS", Name = "Physics", CreatedAt = DateTime.UtcNow }
        };
        context.Departments.AddRange(departments);
        await context.SaveChangesAsync();

        // ==================== COURSES ====================
        var courses = new List<Course>
        {
            // CSE Courses
            new Course { Code = "CS101", Name = "Introduction to Computer Science", CreditHours = 3, DepartmentId = departments[0].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "CS201", Name = "Data Structures", CreditHours = 4, DepartmentId = departments[0].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "CS301", Name = "Algorithms", CreditHours = 4, DepartmentId = departments[0].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "CS401", Name = "Database Systems", CreditHours = 3, DepartmentId = departments[0].Id, CreatedAt = DateTime.UtcNow },
            
            // ENG Courses
            new Course { Code = "ENG101", Name = "Circuit Analysis", CreditHours = 4, DepartmentId = departments[1].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "ENG201", Name = "Digital Logic Design", CreditHours = 4, DepartmentId = departments[1].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "ENG301", Name = "Signal Processing", CreditHours = 3, DepartmentId = departments[1].Id, CreatedAt = DateTime.UtcNow },
            
            // MATH Courses
            new Course { Code = "MATH101", Name = "Calculus I", CreditHours = 4, DepartmentId = departments[2].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "MATH201", Name = "Linear Algebra", CreditHours = 3, DepartmentId = departments[2].Id, CreatedAt = DateTime.UtcNow },
            
            // PHYS Courses
            new Course { Code = "PHYS101", Name = "Physics I", CreditHours = 4, DepartmentId = departments[3].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "PHYS201", Name = "Physics II", CreditHours = 4, DepartmentId = departments[3].Id, CreatedAt = DateTime.UtcNow }
        };
        context.Courses.AddRange(courses);
        await context.SaveChangesAsync();

        // ==================== USERS ====================
        var admin = new User
        {
            Email = "admin@university.edu",
            FirstName = "Admin",
            LastName = "User",
            DepartmentId = departments[0].Id,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123"),
            Role = UserRole.Admin,
            CreatedAt = DateTime.UtcNow
        };

        // Teachers for each department
        var teachers = new List<User>
        {
            // CSE Teachers
            new User { Email = "dr.smith@university.edu", FirstName = "Dr.", LastName = "Smith", DepartmentId = departments[0].Id, PasswordHash = BCrypt.Net.BCrypt.HashPassword("Teacher123"), Role = UserRole.Teacher, CreatedAt = DateTime.UtcNow },
            new User { Email = "dr.johnson@university.edu", FirstName = "Dr.", LastName = "Johnson", DepartmentId = departments[0].Id, PasswordHash = BCrypt.Net.BCrypt.HashPassword("Teacher123"), Role = UserRole.Teacher, CreatedAt = DateTime.UtcNow },
            
            // ENG Teachers
            new User { Email = "dr.brown@university.edu", FirstName = "Dr.", LastName = "Brown", DepartmentId = departments[1].Id, PasswordHash = BCrypt.Net.BCrypt.HashPassword("Teacher123"), Role = UserRole.Teacher, CreatedAt = DateTime.UtcNow },
            new User { Email = "dr.williams@university.edu", FirstName = "Dr.", LastName = "Williams", DepartmentId = departments[1].Id, PasswordHash = BCrypt.Net.BCrypt.HashPassword("Teacher123"), Role = UserRole.Teacher, CreatedAt = DateTime.UtcNow },
            
            // MATH Teachers
            new User { Email = "dr.taylor@university.edu", FirstName = "Dr.", LastName = "Taylor", DepartmentId = departments[2].Id, PasswordHash = BCrypt.Net.BCrypt.HashPassword("Teacher123"), Role = UserRole.Teacher, CreatedAt = DateTime.UtcNow },
            
            // PHYS Teachers
            new User { Email = "dr.miller@university.edu", FirstName = "Dr.", LastName = "Miller", DepartmentId = departments[3].Id, PasswordHash = BCrypt.Net.BCrypt.HashPassword("Teacher123"), Role = UserRole.Teacher, CreatedAt = DateTime.UtcNow }
        };

        // CSE Students
        var cseStudents = new List<User>
        {
            new User { Email = "student1@university.edu", FirstName = "John", LastName = "Doe", DepartmentId = departments[0].Id, PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student123"), Role = UserRole.Student, CreatedAt = DateTime.UtcNow },
            new User { Email = "student2@university.edu", FirstName = "Jane", LastName = "Smith", DepartmentId = departments[0].Id, PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student123"), Role = UserRole.Student, CreatedAt = DateTime.UtcNow },
            new User { Email = "student3@university.edu", FirstName = "Alice", LastName = "Johnson", DepartmentId = departments[0].Id, PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student123"), Role = UserRole.Student, CreatedAt = DateTime.UtcNow }
        };

        // ENG Students
        var engStudents = new List<User>
        {
            new User { Email = "student4@university.edu", FirstName = "Bob", LastName = "Wilson", DepartmentId = departments[1].Id, PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student123"), Role = UserRole.Student, CreatedAt = DateTime.UtcNow },
            new User { Email = "student5@university.edu", FirstName = "Carol", LastName = "Brown", DepartmentId = departments[1].Id, PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student123"), Role = UserRole.Student, CreatedAt = DateTime.UtcNow }
        };

        // MATH Students
        var mathStudents = new List<User>
        {
            new User { Email = "student6@university.edu", FirstName = "David", LastName = "Taylor", DepartmentId = departments[2].Id, PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student123"), Role = UserRole.Student, CreatedAt = DateTime.UtcNow }
        };

        context.Users.Add(admin);
        context.Users.AddRange(teachers);
        context.Users.AddRange(cseStudents);
        context.Users.AddRange(engStudents);
        context.Users.AddRange(mathStudents);
        await context.SaveChangesAsync();

        // ==================== COURSE INSTANCES ====================
        var courseInstances = new List<CourseInstance>
        {
            // CS101 - 2 sections
            new CourseInstance { CourseId = courses[0].Id, TeacherId = teachers[0].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[0].Id, Section = "A", Capacity = 30, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Monday, Day2 = DayOfWeek.Wednesday, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(10, 30), Location = "Room 101", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            new CourseInstance { CourseId = courses[0].Id, TeacherId = teachers[1].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[0].Id, Section = "B", Capacity = 25, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Tuesday, Day2 = DayOfWeek.Thursday, StartTime = new TimeOnly(11, 0), EndTime = new TimeOnly(12, 30), Location = "Room 102", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            
            // CS201
            new CourseInstance { CourseId = courses[1].Id, TeacherId = teachers[0].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[0].Id, Section = "A", Capacity = 28, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Monday, Day2 = DayOfWeek.Wednesday, StartTime = new TimeOnly(14, 0), EndTime = new TimeOnly(15, 30), Location = "Room 201", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            
            // CS301
            new CourseInstance { CourseId = courses[2].Id, TeacherId = teachers[1].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[0].Id, Section = "A", Capacity = 25, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Tuesday, Day2 = DayOfWeek.Thursday, StartTime = new TimeOnly(14, 0), EndTime = new TimeOnly(15, 30), Location = "Room 301", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            
            // ENG101
            new CourseInstance { CourseId = courses[4].Id, TeacherId = teachers[2].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[1].Id, Section = "A", Capacity = 30, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Monday, Day2 = DayOfWeek.Wednesday, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(10, 30), Location = "Lab 101", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            
            // ENG201
            new CourseInstance { CourseId = courses[5].Id, TeacherId = teachers[3].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[1].Id, Section = "A", Capacity = 25, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Tuesday, Day2 = DayOfWeek.Thursday, StartTime = new TimeOnly(11, 0), EndTime = new TimeOnly(12, 30), Location = "Lab 201", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            
            // MATH101
            new CourseInstance { CourseId = courses[7].Id, TeacherId = teachers[4].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[2].Id, Section = "A", Capacity = 35, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Monday, Day2 = DayOfWeek.Wednesday, StartTime = new TimeOnly(11, 0), EndTime = new TimeOnly(12, 30), Location = "Room 501", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            
            // PHYS101
            new CourseInstance { CourseId = courses[9].Id, TeacherId = teachers[5].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[3].Id, Section = "A", Capacity = 30, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Tuesday, Day2 = DayOfWeek.Thursday, StartTime = new TimeOnly(14, 0), EndTime = new TimeOnly(15, 30), Location = "Room 601", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow }
        };
        context.CourseInstances.AddRange(courseInstances);
        await context.SaveChangesAsync();

        // ==================== ENROLLMENTS ====================
        var enrollments = new List<Enrollment>
        {
            // CSE Student 1 enrolled in CS101 Section A and CS201
            new Enrollment { StudentId = cseStudents[0].Id, CourseInstanceId = courseInstances[0].Id, AcademicYearId = academicYear.Id, Status = EnrollmentStatus.Active, EnrolledAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow },
            new Enrollment { StudentId = cseStudents[0].Id, CourseInstanceId = courseInstances[2].Id, AcademicYearId = academicYear.Id, Status = EnrollmentStatus.Active, EnrolledAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow },
            
            // CSE Student 2 enrolled in CS101 Section B and CS301
            new Enrollment { StudentId = cseStudents[1].Id, CourseInstanceId = courseInstances[1].Id, AcademicYearId = academicYear.Id, Status = EnrollmentStatus.Active, EnrolledAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow },
            new Enrollment { StudentId = cseStudents[1].Id, CourseInstanceId = courseInstances[3].Id, AcademicYearId = academicYear.Id, Status = EnrollmentStatus.Active, EnrolledAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow },
            
            // CSE Student 3 enrolled in CS101 Section A
            new Enrollment { StudentId = cseStudents[2].Id, CourseInstanceId = courseInstances[0].Id, AcademicYearId = academicYear.Id, Status = EnrollmentStatus.Active, EnrolledAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow },
            
            // ENG Student 1 enrolled in ENG101 and ENG201
            new Enrollment { StudentId = engStudents[0].Id, CourseInstanceId = courseInstances[4].Id, AcademicYearId = academicYear.Id, Status = EnrollmentStatus.Active, EnrolledAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow },
            new Enrollment { StudentId = engStudents[0].Id, CourseInstanceId = courseInstances[5].Id, AcademicYearId = academicYear.Id, Status = EnrollmentStatus.Active, EnrolledAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow },
            
            // ENG Student 2 enrolled in ENG101
            new Enrollment { StudentId = engStudents[1].Id, CourseInstanceId = courseInstances[4].Id, AcademicYearId = academicYear.Id, Status = EnrollmentStatus.Active, EnrolledAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow },
            
            // MATH Student 1 enrolled in MATH101
            new Enrollment { StudentId = mathStudents[0].Id, CourseInstanceId = courseInstances[6].Id, AcademicYearId = academicYear.Id, Status = EnrollmentStatus.Active, EnrolledAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow }
        };
        context.Enrollments.AddRange(enrollments);

        // Update enrollment counts
        courseInstances[0].CurrentEnrollmentCount = 2;
        courseInstances[1].CurrentEnrollmentCount = 1;
        courseInstances[2].CurrentEnrollmentCount = 1;
        courseInstances[3].CurrentEnrollmentCount = 1;
        courseInstances[4].CurrentEnrollmentCount = 2;
        courseInstances[5].CurrentEnrollmentCount = 1;
        courseInstances[6].CurrentEnrollmentCount = 1;

        await context.SaveChangesAsync();

        // ==================== ANNOUNCEMENTS ====================
        var announcements = new List<Announcement>
        {
            // CS101 Section A announcements
            new Announcement { Title = "Welcome to CS101 Section A", Content = "This is an introductory computer science course. Welcome everyone!", CreatedByTeacherId = teachers[0].Id, CreatedByTeacherName = $"{teachers[0].FirstName} {teachers[0].LastName}", TargetCourseInstanceId = courseInstances[0].Id, PublishedAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow },
            new Announcement { Title = "Assignment 1 Released", Content = "First programming assignment is now available. Due in 2 weeks.", CreatedByTeacherId = teachers[0].Id, CreatedByTeacherName = $"{teachers[0].FirstName} {teachers[0].LastName}", TargetCourseInstanceId = courseInstances[0].Id, PublishedAt = DateTime.UtcNow.AddDays(-2), CreatedAt = DateTime.UtcNow.AddDays(-2) },
            
            // CS101 Section B announcements
            new Announcement { Title = "Welcome to CS101 Section B", Content = "Welcome to the Tuesday/Thursday session of CS101.", CreatedByTeacherId = teachers[1].Id, CreatedByTeacherName = $"{teachers[1].FirstName} {teachers[1].LastName}", TargetCourseInstanceId = courseInstances[1].Id, PublishedAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow },
            
            // CS201 announcements
            new Announcement { Title = "Midterm Exam Announcement", Content = "Midterm exam will be held on December 20th. Study the first 5 chapters.", CreatedByTeacherId = teachers[0].Id, CreatedByTeacherName = $"{teachers[0].FirstName} {teachers[0].LastName}", TargetCourseInstanceId = courseInstances[2].Id, PublishedAt = DateTime.UtcNow.AddDays(-5), CreatedAt = DateTime.UtcNow.AddDays(-5) },
            
            // CS301 announcements
            new Announcement { Title = "Advanced Algorithms Course Started", Content = "Welcome to CS301. This is an advanced course on algorithm design.", CreatedByTeacherId = teachers[1].Id, CreatedByTeacherName = $"{teachers[1].FirstName} {teachers[1].LastName}", TargetCourseInstanceId = courseInstances[3].Id, PublishedAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow },
            
            // ENG101 announcements
            new Announcement { Title = "Circuit Lab Session 1", Content = "Please bring your lab manual to the first lab session.", CreatedByTeacherId = teachers[2].Id, CreatedByTeacherName = $"{teachers[2].FirstName} {teachers[2].LastName}", TargetCourseInstanceId = courseInstances[4].Id, PublishedAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow },
            
            // MATH101 announcements
            new Announcement { Title = "Calculus I Class Starts", Content = "Welcome to Calculus I. This course covers limits, derivatives, and integrals.", CreatedByTeacherId = teachers[4].Id, CreatedByTeacherName = $"{teachers[4].FirstName} {teachers[4].LastName}", TargetCourseInstanceId = courseInstances[6].Id, PublishedAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow }
        };
        context.Announcements.AddRange(announcements);
        await context.SaveChangesAsync();

        // ==================== GRADES ====================
        var grades = new List<Grade>
        {
            // CS101 Section A grades
            new Grade { StudentId = cseStudents[0].Id, CourseInstanceId = courseInstances[0].Id, SubmittedByTeacherId = teachers[0].Id, Score = 85, LetterGrade = "A", SubmittedAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow },
            new Grade { StudentId = cseStudents[2].Id, CourseInstanceId = courseInstances[0].Id, SubmittedByTeacherId = teachers[0].Id, Score = 78, LetterGrade = "B", SubmittedAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow },
            
            // CS101 Section B grades
            new Grade { StudentId = cseStudents[1].Id, CourseInstanceId = courseInstances[1].Id, SubmittedByTeacherId = teachers[1].Id, Score = 92, LetterGrade = "A", SubmittedAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow },
            
            // CS201 grades
            new Grade { StudentId = cseStudents[0].Id, CourseInstanceId = courseInstances[2].Id, SubmittedByTeacherId = teachers[0].Id, Score = 88, LetterGrade = "A", SubmittedAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow },
            
            // ENG101 grades
            new Grade { StudentId = engStudents[0].Id, CourseInstanceId = courseInstances[4].Id, SubmittedByTeacherId = teachers[2].Id, Score = 75, LetterGrade = "C", SubmittedAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow },
            new Grade { StudentId = engStudents[1].Id, CourseInstanceId = courseInstances[4].Id, SubmittedByTeacherId = teachers[2].Id, Score = 82, LetterGrade = "B", SubmittedAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow }
        };
        context.Grades.AddRange(grades);
        await context.SaveChangesAsync();
    }
}