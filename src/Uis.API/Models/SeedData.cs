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
        // 1. Check if data exists
        if (context.Users.Any())
        {
            return;
        }

        // 2. Academic Year
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

        // 3. Departments
        var departments = new List<Department>
        {
            new Department { Code = "CSE", Name = "Computer Science & Engineering", Email = "cse@university.edu", SecretaryEmail = "cse.sekreterlik@university.edu", CreatedAt = DateTime.UtcNow },
            new Department { Code = "ENG", Name = "Electrical Engineering", Email = "eng@university.edu", SecretaryEmail = "eng.sekreterlik@university.edu", CreatedAt = DateTime.UtcNow },
            new Department { Code = "MATH", Name = "Mathematics", Email = "math@university.edu", SecretaryEmail = "math.sekreterlik@university.edu", CreatedAt = DateTime.UtcNow },
            new Department { Code = "PHYS", Name = "Physics", Email = "phys@university.edu", SecretaryEmail = "phys.sekreterlik@university.edu", CreatedAt = DateTime.UtcNow }
        };
        context.Departments.AddRange(departments);
        await context.SaveChangesAsync();

        // 4. Courses - 10 courses per department (40 total)
        var courses = new List<Course>
        {
            // CSE Courses (0-9) - 5 Fall + 5 Spring
            new Course { Code = "CS101", Name = "Introduction to Computer Science", CreditHours = 3, DepartmentId = departments[0].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "CS102", Name = "Programming Fundamentals", CreditHours = 4, DepartmentId = departments[0].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "CS201", Name = "Data Structures", CreditHours = 4, DepartmentId = departments[0].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "CS202", Name = "Object-Oriented Programming", CreditHours = 3, DepartmentId = departments[0].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "CS301", Name = "Algorithms", CreditHours = 4, DepartmentId = departments[0].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "CS302", Name = "Computer Architecture", CreditHours = 3, DepartmentId = departments[0].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "CS401", Name = "Database Systems", CreditHours = 3, DepartmentId = departments[0].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "CS402", Name = "Operating Systems", CreditHours = 4, DepartmentId = departments[0].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "CS501", Name = "Web Development", CreditHours = 3, DepartmentId = departments[0].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "CS502", Name = "Machine Learning", CreditHours = 4, DepartmentId = departments[0].Id, CreatedAt = DateTime.UtcNow },

            // ENG Courses (10-19) - 5 Fall + 5 Spring
            new Course { Code = "ENG101", Name = "Circuit Analysis", CreditHours = 4, DepartmentId = departments[1].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "ENG102", Name = "Electronics Fundamentals", CreditHours = 3, DepartmentId = departments[1].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "ENG201", Name = "Digital Logic Design", CreditHours = 4, DepartmentId = departments[1].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "ENG202", Name = "Microprocessors", CreditHours = 4, DepartmentId = departments[1].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "ENG301", Name = "Signal Processing", CreditHours = 3, DepartmentId = departments[1].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "ENG302", Name = "Communication Systems", CreditHours = 4, DepartmentId = departments[1].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "ENG401", Name = "Power Systems", CreditHours = 3, DepartmentId = departments[1].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "ENG402", Name = "Control Systems", CreditHours = 4, DepartmentId = departments[1].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "ENG501", Name = "VLSI Design", CreditHours = 3, DepartmentId = departments[1].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "ENG502", Name = "Embedded Systems", CreditHours = 4, DepartmentId = departments[1].Id, CreatedAt = DateTime.UtcNow },

            // MATH Courses (20-29) - 5 Fall + 5 Spring
            new Course { Code = "MATH101", Name = "Calculus I", CreditHours = 4, DepartmentId = departments[2].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "MATH102", Name = "Calculus II", CreditHours = 4, DepartmentId = departments[2].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "MATH201", Name = "Linear Algebra", CreditHours = 3, DepartmentId = departments[2].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "MATH202", Name = "Differential Equations", CreditHours = 4, DepartmentId = departments[2].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "MATH301", Name = "Abstract Algebra", CreditHours = 3, DepartmentId = departments[2].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "MATH302", Name = "Real Analysis", CreditHours = 4, DepartmentId = departments[2].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "MATH401", Name = "Complex Analysis", CreditHours = 3, DepartmentId = departments[2].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "MATH402", Name = "Topology", CreditHours = 3, DepartmentId = departments[2].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "MATH501", Name = "Number Theory", CreditHours = 3, DepartmentId = departments[2].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "MATH502", Name = "Numerical Analysis", CreditHours = 4, DepartmentId = departments[2].Id, CreatedAt = DateTime.UtcNow },

            // PHYS Courses (30-39) - 5 Fall + 5 Spring
            new Course { Code = "PHYS101", Name = "Physics I", CreditHours = 4, DepartmentId = departments[3].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "PHYS102", Name = "Physics II", CreditHours = 4, DepartmentId = departments[3].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "PHYS201", Name = "Modern Physics", CreditHours = 3, DepartmentId = departments[3].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "PHYS202", Name = "Thermodynamics", CreditHours = 3, DepartmentId = departments[3].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "PHYS301", Name = "Quantum Mechanics", CreditHours = 4, DepartmentId = departments[3].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "PHYS302", Name = "Electromagnetism", CreditHours = 4, DepartmentId = departments[3].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "PHYS401", Name = "Statistical Mechanics", CreditHours = 3, DepartmentId = departments[3].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "PHYS402", Name = "Solid State Physics", CreditHours = 4, DepartmentId = departments[3].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "PHYS501", Name = "Particle Physics", CreditHours = 3, DepartmentId = departments[3].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "PHYS502", Name = "Astrophysics", CreditHours = 4, DepartmentId = departments[3].Id, CreatedAt = DateTime.UtcNow }
        };
        context.Courses.AddRange(courses);
        await context.SaveChangesAsync();

        // 5. Users (Admin, Teachers, Students - 2 per department)
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

        var teachers = new List<User>
        {
            new User { Email = "dr.smith@university.edu", FirstName = "Dr.", LastName = "Smith", DepartmentId = departments[0].Id, PasswordHash = BCrypt.Net.BCrypt.HashPassword("Teacher123"), Role = UserRole.Teacher, CreatedAt = DateTime.UtcNow },
            new User { Email = "dr.johnson@university.edu", FirstName = "Dr.", LastName = "Johnson", DepartmentId = departments[0].Id, PasswordHash = BCrypt.Net.BCrypt.HashPassword("Teacher123"), Role = UserRole.Teacher, CreatedAt = DateTime.UtcNow },
            new User { Email = "dr.brown@university.edu", FirstName = "Dr.", LastName = "Brown", DepartmentId = departments[1].Id, PasswordHash = BCrypt.Net.BCrypt.HashPassword("Teacher123"), Role = UserRole.Teacher, CreatedAt = DateTime.UtcNow },
            new User { Email = "dr.williams@university.edu", FirstName = "Dr.", LastName = "Williams", DepartmentId = departments[1].Id, PasswordHash = BCrypt.Net.BCrypt.HashPassword("Teacher123"), Role = UserRole.Teacher, CreatedAt = DateTime.UtcNow },
            new User { Email = "dr.taylor@university.edu", FirstName = "Dr.", LastName = "Taylor", DepartmentId = departments[2].Id, PasswordHash = BCrypt.Net.BCrypt.HashPassword("Teacher123"), Role = UserRole.Teacher, CreatedAt = DateTime.UtcNow },
            new User { Email = "dr.miller@university.edu", FirstName = "Dr.", LastName = "Miller", DepartmentId = departments[3].Id, PasswordHash = BCrypt.Net.BCrypt.HashPassword("Teacher123"), Role = UserRole.Teacher, CreatedAt = DateTime.UtcNow }
        };

        // 2 students per department
        var cseStudents = Enumerable.Range(1, 2).Select(i => new User
        {
            Email = $"cse.student{i}@university.edu",
            FirstName = $"CSE_Student",
            LastName = i.ToString(),
            DepartmentId = departments[0].Id,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student123"),
            Role = UserRole.Student,
            PhoneNumber = $"+1-555-010{i:00}",
            DateOfBirth = new DateTime(2003, i % 12 + 1, (i % 28) + 1),
            Address = $"{100 + i} Computer Science Lane",
            City = "Tech City",
            Major = "Computer Science",
            AcademicYear = "2024-2025",
            EmergencyContactName = $"Parent {i}",
            EmergencyContactPhone = $"+1-555-020{i:00}",
            EmergencyContactRelationship = "Parent",
            CreatedAt = DateTime.UtcNow
        }).ToList();

        var engStudents = Enumerable.Range(1, 2).Select(i => new User
        {
            Email = $"eng.student{i}@university.edu",
            FirstName = $"ENG_Student",
            LastName = i.ToString(),
            DepartmentId = departments[1].Id,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student123"),
            Role = UserRole.Student,
            PhoneNumber = $"+1-555-030{i:00}",
            DateOfBirth = new DateTime(2003, (i % 12) + 1, (i % 28) + 1),
            Address = $"{200 + i} Engineering Avenue",
            City = "Tech City",
            Major = "Electrical Engineering",
            AcademicYear = "2024-2025",
            EmergencyContactName = $"Parent {i}",
            EmergencyContactPhone = $"+1-555-040{i:00}",
            EmergencyContactRelationship = "Parent",
            CreatedAt = DateTime.UtcNow
        }).ToList();

        var mathStudents = Enumerable.Range(1, 2).Select(i => new User
        {
            Email = $"math.student{i}@university.edu",
            FirstName = $"MATH_Student",
            LastName = i.ToString(),
            DepartmentId = departments[2].Id,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student123"),
            Role = UserRole.Student,
            PhoneNumber = $"+1-555-050{i:00}",
            DateOfBirth = new DateTime(2003, (i % 12) + 1, (i % 28) + 1),
            Address = $"{300 + i} Math Boulevard",
            City = "Science City",
            Major = "Mathematics",
            AcademicYear = "2024-2025",
            EmergencyContactName = $"Guardian {i}",
            EmergencyContactPhone = $"+1-555-060{i:00}",
            EmergencyContactRelationship = "Guardian",
            CreatedAt = DateTime.UtcNow
        }).ToList();

        var physicsStudents = Enumerable.Range(1, 2).Select(i => new User
        {
            Email = $"phys.student{i}@university.edu",
            FirstName = $"PHYS_Student",
            LastName = i.ToString(),
            DepartmentId = departments[3].Id,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student123"),
            Role = UserRole.Student,
            PhoneNumber = $"+1-555-070{i:00}",
            DateOfBirth = new DateTime(2003, (i % 12) + 1, (i % 28) + 1),
            Address = $"{400 + i} Physics Street",
            City = "Science City",
            Major = "Physics",
            AcademicYear = "2024-2025",
            EmergencyContactName = $"Sibling {i}",
            EmergencyContactPhone = $"+1-555-080{i:00}",
            EmergencyContactRelationship = "Sibling",
            CreatedAt = DateTime.UtcNow
        }).ToList();

        context.Users.Add(admin);
        context.Users.AddRange(teachers);
        context.Users.AddRange(cseStudents);
        context.Users.AddRange(engStudents);
        context.Users.AddRange(mathStudents);
        context.Users.AddRange(physicsStudents);
        await context.SaveChangesAsync();

        // 6. Course Instances - Mix of single and double sections, some with both days, some with only Day1
        var courseInstances = new List<CourseInstance>
        {
            // CSE Courses - 5 Fall (0-4) + 5 Spring (5-9)
            // CS101 - 2 sections
            new CourseInstance { CourseId = courses[0].Id, TeacherId = teachers[0].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[0].Id, Section = "A", Capacity = 30, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Monday, Day2 = DayOfWeek.Wednesday, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(10, 30), Location = "Room 101", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            new CourseInstance { CourseId = courses[0].Id, TeacherId = teachers[1].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[0].Id, Section = "B", Capacity = 25, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Tuesday, Day2 = DayOfWeek.Thursday, StartTime = new TimeOnly(11, 0), EndTime = new TimeOnly(12, 30), Location = "Room 102", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            // CS102 - 1 section with only Day1
            new CourseInstance { CourseId = courses[1].Id, TeacherId = teachers[0].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[0].Id, Section = "A", Capacity = 28, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Friday, Day2 = null, StartTime = new TimeOnly(14, 0), EndTime = new TimeOnly(17, 0), Location = "Room 103", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            // CS201 - 1 section
            new CourseInstance { CourseId = courses[2].Id, TeacherId = teachers[0].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[0].Id, Section = "A", Capacity = 28, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Monday, Day2 = DayOfWeek.Wednesday, StartTime = new TimeOnly(14, 0), EndTime = new TimeOnly(15, 30), Location = "Room 201", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            // CS202 - 1 section
            new CourseInstance { CourseId = courses[3].Id, TeacherId = teachers[1].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[0].Id, Section = "A", Capacity = 25, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Tuesday, Day2 = DayOfWeek.Thursday, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(10, 30), Location = "Room 202", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            // CS301 - 2 sections
            new CourseInstance { CourseId = courses[4].Id, TeacherId = teachers[1].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[0].Id, Section = "A", Capacity = 25, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Tuesday, Day2 = DayOfWeek.Thursday, StartTime = new TimeOnly(14, 0), EndTime = new TimeOnly(15, 30), Location = "Room 301", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            new CourseInstance { CourseId = courses[4].Id, TeacherId = teachers[0].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[0].Id, Section = "B", Capacity = 25, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Monday, Day2 = DayOfWeek.Wednesday, StartTime = new TimeOnly(16, 0), EndTime = new TimeOnly(17, 30), Location = "Room 302", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            // CS302 - 1 section with only Day1
            new CourseInstance { CourseId = courses[5].Id, TeacherId = teachers[0].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[0].Id, Section = "A", Capacity = 30, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Friday, Day2 = null, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(12, 0), Location = "Lab 101", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            // CS401 - 1 section
            new CourseInstance { CourseId = courses[6].Id, TeacherId = teachers[0].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[0].Id, Section = "A", Capacity = 20, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Monday, Day2 = DayOfWeek.Wednesday, StartTime = new TimeOnly(16, 0), EndTime = new TimeOnly(17, 30), Location = "Room 401", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            // CS402 - 1 section
            new CourseInstance { CourseId = courses[7].Id, TeacherId = teachers[1].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[0].Id, Section = "A", Capacity = 22, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Tuesday, Day2 = DayOfWeek.Thursday, StartTime = new TimeOnly(16, 0), EndTime = new TimeOnly(17, 30), Location = "Room 402", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            // CS501 - 1 section
            new CourseInstance { CourseId = courses[8].Id, TeacherId = teachers[1].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[0].Id, Section = "A", Capacity = 20, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Tuesday, Day2 = DayOfWeek.Thursday, StartTime = new TimeOnly(11, 0), EndTime = new TimeOnly(12, 30), Location = "Room 501", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            // CS502 - 1 section with only Day1
            new CourseInstance { CourseId = courses[9].Id, TeacherId = teachers[0].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[0].Id, Section = "A", Capacity = 18, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Friday, Day2 = null, StartTime = new TimeOnly(13, 0), EndTime = new TimeOnly(16, 0), Location = "Lab 201", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },

            // ENG Courses (12-21)
            // ENG101 - 2 sections
            new CourseInstance { CourseId = courses[10].Id, TeacherId = teachers[2].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[1].Id, Section = "A", Capacity = 30, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Monday, Day2 = DayOfWeek.Wednesday, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(10, 30), Location = "Lab 101", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            new CourseInstance { CourseId = courses[10].Id, TeacherId = teachers[3].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[1].Id, Section = "B", Capacity = 28, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Tuesday, Day2 = DayOfWeek.Thursday, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(10, 30), Location = "Lab 102", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            // ENG102 - 1 section
            new CourseInstance { CourseId = courses[11].Id, TeacherId = teachers[2].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[1].Id, Section = "A", Capacity = 25, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Monday, Day2 = DayOfWeek.Wednesday, StartTime = new TimeOnly(14, 0), EndTime = new TimeOnly(15, 30), Location = "Lab 103", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            // ENG201 - 1 section
            new CourseInstance { CourseId = courses[12].Id, TeacherId = teachers[3].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[1].Id, Section = "A", Capacity = 25, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Tuesday, Day2 = DayOfWeek.Thursday, StartTime = new TimeOnly(11, 0), EndTime = new TimeOnly(12, 30), Location = "Lab 201", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            // ENG202 - 1 section with only Day1
            new CourseInstance { CourseId = courses[13].Id, TeacherId = teachers[2].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[1].Id, Section = "A", Capacity = 24, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Friday, Day2 = null, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(12, 0), Location = "Lab 202", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            // ENG301 - 1 section
            new CourseInstance { CourseId = courses[14].Id, TeacherId = teachers[3].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[1].Id, Section = "A", Capacity = 22, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Monday, Day2 = DayOfWeek.Wednesday, StartTime = new TimeOnly(16, 0), EndTime = new TimeOnly(17, 30), Location = "Lab 301", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            // ENG302 - 1 section
            new CourseInstance { CourseId = courses[15].Id, TeacherId = teachers[2].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[1].Id, Section = "A", Capacity = 20, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Tuesday, Day2 = DayOfWeek.Thursday, StartTime = new TimeOnly(14, 0), EndTime = new TimeOnly(15, 30), Location = "Lab 302", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            // ENG401 - 1 section
            new CourseInstance { CourseId = courses[16].Id, TeacherId = teachers[3].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[1].Id, Section = "A", Capacity = 18, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Monday, Day2 = DayOfWeek.Wednesday, StartTime = new TimeOnly(11, 0), EndTime = new TimeOnly(12, 30), Location = "Lab 401", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            // ENG402 - 1 section
            new CourseInstance { CourseId = courses[17].Id, TeacherId = teachers[2].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[1].Id, Section = "A", Capacity = 20, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Tuesday, Day2 = DayOfWeek.Thursday, StartTime = new TimeOnly(16, 0), EndTime = new TimeOnly(17, 30), Location = "Lab 402", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            // ENG501 - 1 section with only Day1
            new CourseInstance { CourseId = courses[18].Id, TeacherId = teachers[3].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[1].Id, Section = "A", Capacity = 15, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Friday, Day2 = null, StartTime = new TimeOnly(13, 0), EndTime = new TimeOnly(16, 0), Location = "Lab 501", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            // ENG502 - 1 section
            new CourseInstance { CourseId = courses[19].Id, TeacherId = teachers[2].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[1].Id, Section = "A", Capacity = 18, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Monday, Day2 = DayOfWeek.Wednesday, StartTime = new TimeOnly(11, 0), EndTime = new TimeOnly(12, 30), Location = "Lab 502", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },

            // MATH Courses (24-33)
            // MATH101 - 2 sections
            new CourseInstance { CourseId = courses[20].Id, TeacherId = teachers[4].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[2].Id, Section = "A", Capacity = 35, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Monday, Day2 = DayOfWeek.Wednesday, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(10, 30), Location = "Room 501", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            new CourseInstance { CourseId = courses[20].Id, TeacherId = teachers[4].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[2].Id, Section = "B", Capacity = 35, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Tuesday, Day2 = DayOfWeek.Thursday, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(10, 30), Location = "Room 502", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            // MATH102 - 1 section
            new CourseInstance { CourseId = courses[21].Id, TeacherId = teachers[4].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[2].Id, Section = "A", Capacity = 32, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Monday, Day2 = DayOfWeek.Wednesday, StartTime = new TimeOnly(14, 0), EndTime = new TimeOnly(15, 30), Location = "Room 503", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            // MATH201 - 1 section
            new CourseInstance { CourseId = courses[22].Id, TeacherId = teachers[4].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[2].Id, Section = "A", Capacity = 25, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Tuesday, Day2 = DayOfWeek.Thursday, StartTime = new TimeOnly(14, 0), EndTime = new TimeOnly(15, 30), Location = "Room 601", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            // MATH202 - 1 section with only Day1
            new CourseInstance { CourseId = courses[23].Id, TeacherId = teachers[4].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[2].Id, Section = "A", Capacity = 25, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Friday, Day2 = null, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(12, 0), Location = "Room 602", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            // MATH301 - 1 section
            new CourseInstance { CourseId = courses[24].Id, TeacherId = teachers[4].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[2].Id, Section = "A", Capacity = 20, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Monday, Day2 = DayOfWeek.Wednesday, StartTime = new TimeOnly(16, 0), EndTime = new TimeOnly(17, 30), Location = "Room 701", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            // MATH302 - 1 section
            new CourseInstance { CourseId = courses[25].Id, TeacherId = teachers[4].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[2].Id, Section = "A", Capacity = 20, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Tuesday, Day2 = DayOfWeek.Thursday, StartTime = new TimeOnly(16, 0), EndTime = new TimeOnly(17, 30), Location = "Room 702", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            // MATH401 - 1 section
            new CourseInstance { CourseId = courses[26].Id, TeacherId = teachers[4].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[2].Id, Section = "A", Capacity = 18, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Monday, Day2 = DayOfWeek.Wednesday, StartTime = new TimeOnly(11, 0), EndTime = new TimeOnly(12, 30), Location = "Room 801", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            // MATH402 - 1 section
            new CourseInstance { CourseId = courses[27].Id, TeacherId = teachers[4].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[2].Id, Section = "A", Capacity = 18, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Tuesday, Day2 = DayOfWeek.Thursday, StartTime = new TimeOnly(11, 0), EndTime = new TimeOnly(12, 30), Location = "Room 802", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            // MATH501 - 1 section
            new CourseInstance { CourseId = courses[28].Id, TeacherId = teachers[4].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[2].Id, Section = "A", Capacity = 15, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Monday, Day2 = DayOfWeek.Wednesday, StartTime = new TimeOnly(13, 0), EndTime = new TimeOnly(14, 30), Location = "Room 901", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            // MATH502 - 1 section with only Day1
            new CourseInstance { CourseId = courses[29].Id, TeacherId = teachers[4].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[2].Id, Section = "A", Capacity = 15, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Friday, Day2 = null, StartTime = new TimeOnly(13, 0), EndTime = new TimeOnly(16, 0), Location = "Room 902", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },

            // PHYS Courses (36-45)
            // PHYS101 - 2 sections
            new CourseInstance { CourseId = courses[30].Id, TeacherId = teachers[5].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[3].Id, Section = "A", Capacity = 30, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Tuesday, Day2 = DayOfWeek.Thursday, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(10, 30), Location = "Room 701", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            new CourseInstance { CourseId = courses[30].Id, TeacherId = teachers[5].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[3].Id, Section = "B", Capacity = 28, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Monday, Day2 = DayOfWeek.Wednesday, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(10, 30), Location = "Room 702", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            // PHYS102 - 1 section
            new CourseInstance { CourseId = courses[31].Id, TeacherId = teachers[5].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[3].Id, Section = "A", Capacity = 28, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Tuesday, Day2 = DayOfWeek.Thursday, StartTime = new TimeOnly(14, 0), EndTime = new TimeOnly(15, 30), Location = "Room 703", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            // PHYS201 - 1 section
            new CourseInstance { CourseId = courses[32].Id, TeacherId = teachers[5].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[3].Id, Section = "A", Capacity = 25, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Monday, Day2 = DayOfWeek.Wednesday, StartTime = new TimeOnly(14, 0), EndTime = new TimeOnly(15, 30), Location = "Room 801", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            // PHYS202 - 1 section with only Day1
            new CourseInstance { CourseId = courses[33].Id, TeacherId = teachers[5].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[3].Id, Section = "A", Capacity = 24, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Friday, Day2 = null, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(12, 0), Location = "Lab 801", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            // PHYS301 - 1 section
            new CourseInstance { CourseId = courses[34].Id, TeacherId = teachers[5].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[3].Id, Section = "A", Capacity = 20, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Tuesday, Day2 = DayOfWeek.Thursday, StartTime = new TimeOnly(16, 0), EndTime = new TimeOnly(17, 30), Location = "Room 901", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            // PHYS302 - 1 section
            new CourseInstance { CourseId = courses[35].Id, TeacherId = teachers[5].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[3].Id, Section = "A", Capacity = 20, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Monday, Day2 = DayOfWeek.Wednesday, StartTime = new TimeOnly(16, 0), EndTime = new TimeOnly(17, 30), Location = "Room 902", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            // PHYS401 - 1 section
            new CourseInstance { CourseId = courses[36].Id, TeacherId = teachers[5].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[3].Id, Section = "A", Capacity = 18, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Tuesday, Day2 = DayOfWeek.Thursday, StartTime = new TimeOnly(11, 0), EndTime = new TimeOnly(12, 30), Location = "Lab 901", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            // PHYS402 - 1 section
            new CourseInstance { CourseId = courses[37].Id, TeacherId = teachers[5].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[3].Id, Section = "A", Capacity = 18, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Monday, Day2 = DayOfWeek.Wednesday, StartTime = new TimeOnly(11, 0), EndTime = new TimeOnly(12, 30), Location = "Lab 902", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            // PHYS501 - 1 section
            new CourseInstance { CourseId = courses[38].Id, TeacherId = teachers[5].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[3].Id, Section = "A", Capacity = 15, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Monday, Day2 = DayOfWeek.Wednesday, StartTime = new TimeOnly(13, 0), EndTime = new TimeOnly(14, 30), Location = "Lab 1001", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow },
            // PHYS502 - 1 section with only Day1
            new CourseInstance { CourseId = courses[39].Id, TeacherId = teachers[5].Id, AcademicYearId = academicYear.Id, DepartmentId = departments[3].Id, Section = "A", Capacity = 15, CurrentEnrollmentCount = 0, Day1 = DayOfWeek.Friday, Day2 = null, StartTime = new TimeOnly(13, 0), EndTime = new TimeOnly(16, 0), Location = "Lab 1002", Status = CourseInstanceStatus.Open, ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow }
        };
        context.CourseInstances.AddRange(courseInstances);
        await context.SaveChangesAsync();

        // 7. Enrollments - Each student enrolls in a few courses from their department
        var enrollments = new List<Enrollment>();

        // CSE Students - enroll in first few CS courses
        enrollments.Add(new Enrollment { StudentId = cseStudents[0].Id, CourseInstanceId = courseInstances[0].Id, AcademicYearId = academicYear.Id, Status = EnrollmentStatus.Active, EnrolledAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow });
        enrollments.Add(new Enrollment { StudentId = cseStudents[0].Id, CourseInstanceId = courseInstances[2].Id, AcademicYearId = academicYear.Id, Status = EnrollmentStatus.Active, EnrolledAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow });
        enrollments.Add(new Enrollment { StudentId = cseStudents[0].Id, CourseInstanceId = courseInstances[3].Id, AcademicYearId = academicYear.Id, Status = EnrollmentStatus.Active, EnrolledAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow });

        enrollments.Add(new Enrollment { StudentId = cseStudents[1].Id, CourseInstanceId = courseInstances[1].Id, AcademicYearId = academicYear.Id, Status = EnrollmentStatus.Active, EnrolledAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow });
        enrollments.Add(new Enrollment { StudentId = cseStudents[1].Id, CourseInstanceId = courseInstances[2].Id, AcademicYearId = academicYear.Id, Status = EnrollmentStatus.Active, EnrolledAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow });
        enrollments.Add(new Enrollment { StudentId = cseStudents[1].Id, CourseInstanceId = courseInstances[4].Id, AcademicYearId = academicYear.Id, Status = EnrollmentStatus.Active, EnrolledAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow });

        // ENG Students
        enrollments.Add(new Enrollment { StudentId = engStudents[0].Id, CourseInstanceId = courseInstances[12].Id, AcademicYearId = academicYear.Id, Status = EnrollmentStatus.Active, EnrolledAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow });
        enrollments.Add(new Enrollment { StudentId = engStudents[0].Id, CourseInstanceId = courseInstances[14].Id, AcademicYearId = academicYear.Id, Status = EnrollmentStatus.Active, EnrolledAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow });
        enrollments.Add(new Enrollment { StudentId = engStudents[0].Id, CourseInstanceId = courseInstances[15].Id, AcademicYearId = academicYear.Id, Status = EnrollmentStatus.Active, EnrolledAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow });

        enrollments.Add(new Enrollment { StudentId = engStudents[1].Id, CourseInstanceId = courseInstances[13].Id, AcademicYearId = academicYear.Id, Status = EnrollmentStatus.Active, EnrolledAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow });
        enrollments.Add(new Enrollment { StudentId = engStudents[1].Id, CourseInstanceId = courseInstances[15].Id, AcademicYearId = academicYear.Id, Status = EnrollmentStatus.Active, EnrolledAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow });
        enrollments.Add(new Enrollment { StudentId = engStudents[1].Id, CourseInstanceId = courseInstances[16].Id, AcademicYearId = academicYear.Id, Status = EnrollmentStatus.Active, EnrolledAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow });

        // MATH Students
        enrollments.Add(new Enrollment { StudentId = mathStudents[0].Id, CourseInstanceId = courseInstances[24].Id, AcademicYearId = academicYear.Id, Status = EnrollmentStatus.Active, EnrolledAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow });
        enrollments.Add(new Enrollment { StudentId = mathStudents[0].Id, CourseInstanceId = courseInstances[26].Id, AcademicYearId = academicYear.Id, Status = EnrollmentStatus.Active, EnrolledAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow });
        enrollments.Add(new Enrollment { StudentId = mathStudents[0].Id, CourseInstanceId = courseInstances[27].Id, AcademicYearId = academicYear.Id, Status = EnrollmentStatus.Active, EnrolledAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow });

        enrollments.Add(new Enrollment { StudentId = mathStudents[1].Id, CourseInstanceId = courseInstances[25].Id, AcademicYearId = academicYear.Id, Status = EnrollmentStatus.Active, EnrolledAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow });
        enrollments.Add(new Enrollment { StudentId = mathStudents[1].Id, CourseInstanceId = courseInstances[27].Id, AcademicYearId = academicYear.Id, Status = EnrollmentStatus.Active, EnrolledAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow });
        enrollments.Add(new Enrollment { StudentId = mathStudents[1].Id, CourseInstanceId = courseInstances[28].Id, AcademicYearId = academicYear.Id, Status = EnrollmentStatus.Active, EnrolledAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow });

        // PHYS Students
        enrollments.Add(new Enrollment { StudentId = physicsStudents[0].Id, CourseInstanceId = courseInstances[36].Id, AcademicYearId = academicYear.Id, Status = EnrollmentStatus.Active, EnrolledAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow });
        enrollments.Add(new Enrollment { StudentId = physicsStudents[0].Id, CourseInstanceId = courseInstances[38].Id, AcademicYearId = academicYear.Id, Status = EnrollmentStatus.Active, EnrolledAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow });
        enrollments.Add(new Enrollment { StudentId = physicsStudents[0].Id, CourseInstanceId = courseInstances[40].Id, AcademicYearId = academicYear.Id, Status = EnrollmentStatus.Active, EnrolledAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow });

        enrollments.Add(new Enrollment { StudentId = physicsStudents[1].Id, CourseInstanceId = courseInstances[37].Id, AcademicYearId = academicYear.Id, Status = EnrollmentStatus.Active, EnrolledAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow });
        enrollments.Add(new Enrollment { StudentId = physicsStudents[1].Id, CourseInstanceId = courseInstances[39].Id, AcademicYearId = academicYear.Id, Status = EnrollmentStatus.Active, EnrolledAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow });
        enrollments.Add(new Enrollment { StudentId = physicsStudents[1].Id, CourseInstanceId = courseInstances[41].Id, AcademicYearId = academicYear.Id, Status = EnrollmentStatus.Active, EnrolledAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow });

        context.Enrollments.AddRange(enrollments);

        // Update enrollment counts
        foreach (var ci in courseInstances)
        {
            ci.CurrentEnrollmentCount = enrollments.Count(e => e.CourseInstanceId == ci.Id);
        }
        await context.SaveChangesAsync();

        // 8. Announcements
        var announcements = new List<Announcement>
        {
            new Announcement { Title = "Welcome to CS101 Section A", Content = "Welcome to Introduction to Computer Science.", CreatedByTeacherId = teachers[0].Id, CreatedByTeacherName = "Dr. Smith", TargetCourseInstanceId = courseInstances[0].Id, PublishedAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow },
            new Announcement { Title = "CS201 Data Structures Begins", Content = "Welcome to Data Structures course.", CreatedByTeacherId = teachers[0].Id, CreatedByTeacherName = "Dr. Smith", TargetCourseInstanceId = courseInstances[3].Id, PublishedAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow },
            new Announcement { Title = "ENG101 Circuit Lab Safety", Content = "Wear safety goggles in all labs.", CreatedByTeacherId = teachers[2].Id, CreatedByTeacherName = "Dr. Brown", TargetCourseInstanceId = courseInstances[12].Id, PublishedAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow },
            new Announcement { Title = "MATH101 Calculus Starts", Content = "Welcome to Calculus I.", CreatedByTeacherId = teachers[4].Id, CreatedByTeacherName = "Dr. Taylor", TargetCourseInstanceId = courseInstances[24].Id, PublishedAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow },
            new Announcement { Title = "PHYS101 Welcome", Content = "Welcome to Physics I.", CreatedByTeacherId = teachers[5].Id, CreatedByTeacherName = "Dr. Miller", TargetCourseInstanceId = courseInstances[36].Id, PublishedAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow }
        };
        context.Announcements.AddRange(announcements);
        await context.SaveChangesAsync();

        // 9. Grades
        var grades = new List<Grade>();
        // CS101 Section A - cseStudents[0]
        var score1 = CalculateScore(85, 87, 90, 88);
        grades.Add(new Grade { StudentId = cseStudents[0].Id, CourseInstanceId = courseInstances[0].Id, SubmittedByTeacherId = teachers[0].Id, Exam1 = 85, Exam2 = 87, Final = 90, Project = 88, Score = score1, LetterGrade = CalculateLetterGrade(score1), SubmittedAt = DateTime.UtcNow.AddDays(-10), CreatedAt = DateTime.UtcNow.AddDays(-10) });

        // CS101 Section B - cseStudents[1]
        var score2 = CalculateScore(80, 83, 85, 82);
        grades.Add(new Grade { StudentId = cseStudents[1].Id, CourseInstanceId = courseInstances[1].Id, SubmittedByTeacherId = teachers[1].Id, Exam1 = 80, Exam2 = 83, Final = 85, Project = 82, Score = score2, LetterGrade = CalculateLetterGrade(score2), SubmittedAt = DateTime.UtcNow.AddDays(-10), CreatedAt = DateTime.UtcNow.AddDays(-10) });

        // ENG101 - engStudents[0]
        var score3 = CalculateScore(78, 80, 82, 85);
        grades.Add(new Grade { StudentId = engStudents[0].Id, CourseInstanceId = courseInstances[12].Id, SubmittedByTeacherId = teachers[2].Id, Exam1 = 78, Exam2 = 80, Final = 82, Project = 85, Score = score3, LetterGrade = CalculateLetterGrade(score3), SubmittedAt = DateTime.UtcNow.AddDays(-7), CreatedAt = DateTime.UtcNow.AddDays(-7) });

        // MATH101 - mathStudents[0]
        var score4 = CalculateScore(88, 90, 92, null);
        grades.Add(new Grade { StudentId = mathStudents[0].Id, CourseInstanceId = courseInstances[24].Id, SubmittedByTeacherId = teachers[4].Id, Exam1 = 88, Exam2 = 90, Final = 92, Project = null, Score = score4, LetterGrade = CalculateLetterGrade(score4), SubmittedAt = DateTime.UtcNow.AddDays(-5), CreatedAt = DateTime.UtcNow.AddDays(-5) });

        // PHYS101 - physicsStudents[0]
        var score5 = CalculateScore(82, 84, 86, 88);
        grades.Add(new Grade { StudentId = physicsStudents[0].Id, CourseInstanceId = courseInstances[36].Id, SubmittedByTeacherId = teachers[5].Id, Exam1 = 82, Exam2 = 84, Final = 86, Project = 88, Score = score5, LetterGrade = CalculateLetterGrade(score5), SubmittedAt = DateTime.UtcNow.AddDays(-3), CreatedAt = DateTime.UtcNow.AddDays(-3) });

        context.Grades.AddRange(grades);
        await context.SaveChangesAsync();

        // 10. Attendances
        var attendances = new List<Attendance>();
        var random = new Random();
        foreach (var enrollment in enrollments)
        {
            var courseInstance = courseInstances.FirstOrDefault(ci => ci.Id == enrollment.CourseInstanceId);
            if (courseInstance == null) continue;
            for (int week = 1; week <= 12; week++)
            {
                attendances.Add(new Attendance { EnrollmentId = enrollment.Id, Week = week, Day = courseInstance.Day1, Status = random.Next(0, 100) >= 90 ? "Absent" : "Present", CreatedAt = DateTime.UtcNow });
                if (courseInstance.Day2.HasValue)
                    attendances.Add(new Attendance { EnrollmentId = enrollment.Id, Week = week, Day = courseInstance.Day2.Value, Status = random.Next(0, 100) >= 90 ? "Absent" : "Present", CreatedAt = DateTime.UtcNow });
            }
        }
        context.Attendances.AddRange(attendances);
        await context.SaveChangesAsync();

        // 11. Assignments
        if (!context.Assignments.Any())
        {
            var assignments = new List<Assignment>
            {
                // CS101 Section A (Smith)
                new Assignment
                {
                    CourseInstanceId = courseInstances[0].Id,
                    CreatedByTeacherId = teachers[0].Id,
                    Title = "Variables and Data Types",
                    Description = "Write a program demonstrating the use of different data types in C#. Include examples of int, string, bool, double, and char variables.",
                    DueDate = DateTime.UtcNow.AddDays(7),
                    TotalPoints = 100,
                    Status = AssignmentStatus.Published,
                    CreatedAt = DateTime.UtcNow.AddDays(-14)
                },
                new Assignment
                {
                    CourseInstanceId = courseInstances[0].Id,
                    CreatedByTeacherId = teachers[0].Id,
                    Title = "Control Flow and Loops",
                    Description = "Implement a program using if-else statements, switch cases, for loops, and while loops. Solve at least 5 different problems.",
                    DueDate = DateTime.UtcNow.AddDays(14),
                    TotalPoints = 150,
                    Status = AssignmentStatus.Published,
                    CreatedAt = DateTime.UtcNow.AddDays(-10)
                },

                // CS201 (Smith)
                new Assignment
                {
                    CourseInstanceId = courseInstances[3].Id,
                    CreatedByTeacherId = teachers[0].Id,
                    Title = "Linked Lists Implementation",
                    Description = "Implement singly and doubly linked lists with add, remove, and search operations.",
                    DueDate = DateTime.UtcNow.AddDays(10),
                    TotalPoints = 120,
                    Status = AssignmentStatus.Published,
                    CreatedAt = DateTime.UtcNow.AddDays(-8)
                },

                // ENG101 (Brown)
                new Assignment
                {
                    CourseInstanceId = courseInstances[12].Id,
                    CreatedByTeacherId = teachers[2].Id,
                    Title = "Circuit Analysis Problems",
                    Description = "Solve 10 circuit analysis problems using Kirchhoff's laws.",
                    DueDate = DateTime.UtcNow.AddDays(10),
                    TotalPoints = 100,
                    Status = AssignmentStatus.Published,
                    CreatedAt = DateTime.UtcNow.AddDays(-11)
                },

                // MATH101 (Taylor)
                new Assignment
                {
                    CourseInstanceId = courseInstances[24].Id,
                    CreatedByTeacherId = teachers[4].Id,
                    Title = "Limits and Continuity",
                    Description = "Solve 15 problems on limits and continuity. Show all work and explain your reasoning.",
                    DueDate = DateTime.UtcNow.AddDays(6),
                    TotalPoints = 100,
                    Status = AssignmentStatus.Published,
                    CreatedAt = DateTime.UtcNow.AddDays(-15)
                },
                new Assignment
                {
                    CourseInstanceId = courseInstances[24].Id,
                    CreatedByTeacherId = teachers[4].Id,
                    Title = "Derivatives Practice",
                    Description = "Calculate derivatives for 20 different functions using various differentiation rules.",
                    DueDate = DateTime.UtcNow.AddDays(12),
                    TotalPoints = 150,
                    Status = AssignmentStatus.Published,
                    CreatedAt = DateTime.UtcNow.AddDays(-9)
                },

                // PHYS101 (Miller)
                new Assignment
                {
                    CourseInstanceId = courseInstances[36].Id,
                    CreatedByTeacherId = teachers[5].Id,
                    Title = "Newton's Laws of Motion",
                    Description = "Solve 10 problems applying Newton's three laws of motion. Include free body diagrams.",
                    DueDate = DateTime.UtcNow.AddDays(8),
                    TotalPoints = 100,
                    Status = AssignmentStatus.Published,
                    CreatedAt = DateTime.UtcNow.AddDays(-13)
                }
            };

            context.Assignments.AddRange(assignments);
            await context.SaveChangesAsync();

            var submissions = new List<AssignmentSubmission>
            {
                // cseStudents[0] submissions
                new AssignmentSubmission
                {
                    AssignmentId = assignments[0].Id,
                    StudentId = cseStudents[0].Id,
                    SubmissionText = "I have completed the variables assignment. My program demonstrates int, string, bool, double, and char data types with proper examples and comments.",
                    SubmittedAt = DateTime.UtcNow.AddDays(-3),
                    Grade = 95,
                    GradedAt = DateTime.UtcNow.AddDays(-1),
                    GradedByTeacherId = teachers[0].Id,
                    Status = SubmissionStatus.Graded,
                    CreatedAt = DateTime.UtcNow.AddDays(-3)
                },

                // cseStudents[1] submissions
                new AssignmentSubmission
                {
                    AssignmentId = assignments[0].Id,
                    StudentId = cseStudents[1].Id,
                    SubmissionText = "Completed assignment on data types. Included examples and explanations for each type.",
                    SubmittedAt = DateTime.UtcNow.AddDays(-4),
                    Grade = 88,
                    GradedAt = DateTime.UtcNow.AddDays(-2),
                    GradedByTeacherId = teachers[0].Id,
                    Status = SubmissionStatus.Graded,
                    CreatedAt = DateTime.UtcNow.AddDays(-4)
                },

                // mathStudents[0] submissions
                new AssignmentSubmission
                {
                    AssignmentId = assignments[4].Id,
                    StudentId = mathStudents[0].Id,
                    SubmissionText = "Solved all 15 problems on limits and continuity with detailed work shown.",
                    SubmittedAt = DateTime.UtcNow.AddDays(-5),
                    Grade = 92,
                    GradedAt = DateTime.UtcNow.AddDays(-3),
                    GradedByTeacherId = teachers[4].Id,
                    Status = SubmissionStatus.Graded,
                    CreatedAt = DateTime.UtcNow.AddDays(-5)
                },

                // physicsStudents[0] submissions
                new AssignmentSubmission
                {
                    AssignmentId = assignments[6].Id,
                    StudentId = physicsStudents[0].Id,
                    SubmissionText = "Completed Newton's Laws problems with free body diagrams.",
                    SubmittedAt = DateTime.UtcNow.AddDays(-1),
                    Status = SubmissionStatus.Submitted,
                    CreatedAt = DateTime.UtcNow.AddDays(-1)
                }
            };

            context.AssignmentSubmissions.AddRange(submissions);
            await context.SaveChangesAsync();
        }
    }

    private static decimal CalculateScore(int? exam1, int? exam2, int? final, int? project)
    {
        decimal score = 0;
        int componentCount = 0;

        if (exam1.HasValue) { score += exam1.Value * 0.2m; componentCount++; }
        if (exam2.HasValue) { score += exam2.Value * 0.2m; componentCount++; }
        if (final.HasValue) { score += final.Value * 0.4m; componentCount++; }
        if (project.HasValue) { score += project.Value * 0.2m; componentCount++; }

        return componentCount > 0 ? Math.Round(score, 2) : 0;
    }

    private static string CalculateLetterGrade(decimal score)
    {
        if (score >= 90) return "A";
        if (score >= 85) return "A-";
        if (score >= 80) return "B+";
        if (score >= 75) return "B";
        if (score >= 70) return "B-";
        if (score >= 65) return "C+";
        if (score >= 60) return "C";
        if (score >= 55) return "C-";
        if (score >= 50) return "D";
        return "F";
    }
}