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

        // 4. Courses
        var courses = new List<Course>
        {
            // CSE Courses (Indices 0-4)
            new Course { Code = "CS101", Name = "Introduction to Computer Science", CreditHours = 3, DepartmentId = departments[0].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "CS201", Name = "Data Structures", CreditHours = 4, DepartmentId = departments[0].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "CS301", Name = "Algorithms", CreditHours = 4, DepartmentId = departments[0].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "CS401", Name = "Database Systems", CreditHours = 3, DepartmentId = departments[0].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "CS501", Name = "Web Development", CreditHours = 3, DepartmentId = departments[0].Id, CreatedAt = DateTime.UtcNow },

            // ENG Courses (Indices 5-7)
            new Course { Code = "ENG101", Name = "Circuit Analysis", CreditHours = 4, DepartmentId = departments[1].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "ENG201", Name = "Digital Logic Design", CreditHours = 4, DepartmentId = departments[1].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "ENG301", Name = "Signal Processing", CreditHours = 3, DepartmentId = departments[1].Id, CreatedAt = DateTime.UtcNow },

            // MATH Courses (Indices 8-9)
            new Course { Code = "MATH101", Name = "Calculus I", CreditHours = 4, DepartmentId = departments[2].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "MATH201", Name = "Linear Algebra", CreditHours = 3, DepartmentId = departments[2].Id, CreatedAt = DateTime.UtcNow },

            // PHYS Courses (Indices 10-11)
            new Course { Code = "PHYS101", Name = "Physics I", CreditHours = 4, DepartmentId = departments[3].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "PHYS201", Name = "Physics II", CreditHours = 4, DepartmentId = departments[3].Id, CreatedAt = DateTime.UtcNow }
        };
        context.Courses.AddRange(courses);
        await context.SaveChangesAsync();

        // 5. Users (Admin, Teachers, Students)
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

        var cseStudents = Enumerable.Range(1, 10).Select(i => new User
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

        var engStudents = Enumerable.Range(1, 8).Select(i => new User
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

        var mathStudents = Enumerable.Range(1, 5).Select(i => new User
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

        var physicsStudents = Enumerable.Range(1, 5).Select(i => new User
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

        // 6. Course Instances
        var courseInstances = new List<CourseInstance>
        {
            // 0: CS101 Section A (Dr. Smith)
            new CourseInstance
            {
                CourseId = courses[0].Id, TeacherId = teachers[0].Id, AcademicYearId = academicYear.Id,
                DepartmentId = departments[0].Id, Section = "A", Capacity = 30, CurrentEnrollmentCount = 0,
                Day1 = DayOfWeek.Monday, Day2 = DayOfWeek.Wednesday, StartTime = new TimeOnly(9, 0),
                EndTime = new TimeOnly(10, 30), Location = "Room 101", Status = CourseInstanceStatus.Open,
                ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow
            },
            // 1: CS101 Section B (Dr. Johnson)
            new CourseInstance
            {
                CourseId = courses[0].Id, TeacherId = teachers[1].Id, AcademicYearId = academicYear.Id,
                DepartmentId = departments[0].Id, Section = "B", Capacity = 25, CurrentEnrollmentCount = 0,
                Day1 = DayOfWeek.Tuesday, Day2 = DayOfWeek.Thursday, StartTime = new TimeOnly(11, 0),
                EndTime = new TimeOnly(12, 30), Location = "Room 102", Status = CourseInstanceStatus.Open,
                ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow
            },
            // 2: CS201 Section A
            new CourseInstance
            {
                CourseId = courses[1].Id, TeacherId = teachers[0].Id, AcademicYearId = academicYear.Id,
                DepartmentId = departments[0].Id, Section = "A", Capacity = 28, CurrentEnrollmentCount = 0,
                Day1 = DayOfWeek.Monday, Day2 = DayOfWeek.Wednesday, StartTime = new TimeOnly(14, 0),
                EndTime = new TimeOnly(15, 30), Location = "Room 201", Status = CourseInstanceStatus.Open,
                ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow
            },
            // 3: CS301 Section A
            new CourseInstance
            {
                CourseId = courses[2].Id, TeacherId = teachers[1].Id, AcademicYearId = academicYear.Id,
                DepartmentId = departments[0].Id, Section = "A", Capacity = 25, CurrentEnrollmentCount = 0,
                Day1 = DayOfWeek.Tuesday, Day2 = DayOfWeek.Thursday, StartTime = new TimeOnly(14, 0),
                EndTime = new TimeOnly(15, 30), Location = "Room 301", Status = CourseInstanceStatus.Open,
                ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow
            },
            // 4: CS401 Section A
            new CourseInstance
            {
                CourseId = courses[3].Id, TeacherId = teachers[0].Id, AcademicYearId = academicYear.Id,
                DepartmentId = departments[0].Id, Section = "A", Capacity = 20, CurrentEnrollmentCount = 0,
                Day1 = DayOfWeek.Monday, Day2 = DayOfWeek.Wednesday, StartTime = new TimeOnly(16, 0),
                EndTime = new TimeOnly(17, 30), Location = "Room 401", Status = CourseInstanceStatus.Open,
                ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow
            },
            // 5: CS501 Section A
            new CourseInstance
            {
                CourseId = courses[4].Id, TeacherId = teachers[1].Id, AcademicYearId = academicYear.Id,
                DepartmentId = departments[0].Id, Section = "A", Capacity = 5, CurrentEnrollmentCount = 0,
                Day1 = DayOfWeek.Tuesday, Day2 = DayOfWeek.Thursday, StartTime = new TimeOnly(11, 0),
                EndTime = new TimeOnly(12, 30), Location = "Room 501", Status = CourseInstanceStatus.Open,
                ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow
            },
            // 6: ENG101 Section A
            new CourseInstance
            {
                CourseId = courses[5].Id, TeacherId = teachers[2].Id, AcademicYearId = academicYear.Id,
                DepartmentId = departments[1].Id, Section = "A", Capacity = 30, CurrentEnrollmentCount = 0,
                Day1 = DayOfWeek.Monday, Day2 = DayOfWeek.Wednesday, StartTime = new TimeOnly(9, 0),
                EndTime = new TimeOnly(10, 30), Location = "Lab 101", Status = CourseInstanceStatus.Open,
                ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow
            },
            // 7: ENG201 Section A
            new CourseInstance
            {
                CourseId = courses[6].Id, TeacherId = teachers[3].Id, AcademicYearId = academicYear.Id,
                DepartmentId = departments[1].Id, Section = "A", Capacity = 25, CurrentEnrollmentCount = 0,
                Day1 = DayOfWeek.Tuesday, Day2 = DayOfWeek.Thursday, StartTime = new TimeOnly(11, 0),
                EndTime = new TimeOnly(12, 30), Location = "Lab 201", Status = CourseInstanceStatus.Open,
                ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow
            },
            // 8: MATH101 Section A
            new CourseInstance
            {
                CourseId = courses[8].Id, TeacherId = teachers[4].Id, AcademicYearId = academicYear.Id,
                DepartmentId = departments[2].Id, Section = "A", Capacity = 35, CurrentEnrollmentCount = 0,
                Day1 = DayOfWeek.Monday, Day2 = DayOfWeek.Wednesday, StartTime = new TimeOnly(11, 0),
                EndTime = new TimeOnly(12, 30), Location = "Room 501", Status = CourseInstanceStatus.Open,
                ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow
            },
            // 9: MATH201 Section A
            new CourseInstance
            {
                CourseId = courses[9].Id, TeacherId = teachers[4].Id, AcademicYearId = academicYear.Id,
                DepartmentId = departments[2].Id, Section = "A", Capacity = 25, CurrentEnrollmentCount = 0,
                Day1 = DayOfWeek.Tuesday, Day2 = DayOfWeek.Thursday, StartTime = new TimeOnly(14, 0),
                EndTime = new TimeOnly(15, 30), Location = "Room 601", Status = CourseInstanceStatus.Open,
                ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow
            },
            // 10: PHYS101 Section A
            new CourseInstance
            {
                CourseId = courses[10].Id, TeacherId = teachers[5].Id, AcademicYearId = academicYear.Id,
                DepartmentId = departments[3].Id, Section = "A", Capacity = 30, CurrentEnrollmentCount = 0,
                Day1 = DayOfWeek.Tuesday, Day2 = DayOfWeek.Thursday, StartTime = new TimeOnly(14, 0),
                EndTime = new TimeOnly(15, 30), Location = "Room 701", Status = CourseInstanceStatus.Open,
                ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow
            }
        };
        context.CourseInstances.AddRange(courseInstances);
        await context.SaveChangesAsync();

        // 7. Enrollments
        var enrollments = new List<Enrollment>();

        // CSE Students
        for (int i = 0; i < cseStudents.Count; i++)
        {
            enrollments.Add(new Enrollment
            {
                StudentId = cseStudents[i].Id,
                CourseInstanceId = courseInstances[i % 2].Id,
                AcademicYearId = academicYear.Id,
                Status = EnrollmentStatus.Active,
                EnrolledAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            });
            if (i < 5)
            {
                enrollments.Add(new Enrollment { StudentId = cseStudents[i].Id, CourseInstanceId = courseInstances[2].Id, AcademicYearId = academicYear.Id, Status = EnrollmentStatus.Active, EnrolledAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow });
            }
        }
        // ENG Students
        for (int i = 0; i < engStudents.Count; i++)
        {
            enrollments.Add(new Enrollment { StudentId = engStudents[i].Id, CourseInstanceId = courseInstances[6].Id, AcademicYearId = academicYear.Id, Status = EnrollmentStatus.Active, EnrolledAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow });
        }
        // MATH Students
        for (int i = 0; i < mathStudents.Count; i++)
        {
            enrollments.Add(new Enrollment { StudentId = mathStudents[i].Id, CourseInstanceId = courseInstances[8].Id, AcademicYearId = academicYear.Id, Status = EnrollmentStatus.Active, EnrolledAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow });
        }
        // PHYS Students
        for (int i = 0; i < physicsStudents.Count; i++)
        {
            enrollments.Add(new Enrollment { StudentId = physicsStudents[i].Id, CourseInstanceId = courseInstances[10].Id, AcademicYearId = academicYear.Id, Status = EnrollmentStatus.Active, EnrolledAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow });
        }
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
            new Announcement { Title = "Assignment 1", Content = "Write a Hello World program.", CreatedByTeacherId = teachers[0].Id, CreatedByTeacherName = "Dr. Smith", TargetCourseInstanceId = courseInstances[0].Id, PublishedAt = DateTime.UtcNow.AddDays(-3), CreatedAt = DateTime.UtcNow.AddDays(-3) },
            new Announcement { Title = "Quiz 1 Results", Content = "Quiz 1 has been graded.", CreatedByTeacherId = teachers[0].Id, CreatedByTeacherName = "Dr. Smith", TargetCourseInstanceId = courseInstances[0].Id, PublishedAt = DateTime.UtcNow.AddDays(-7), CreatedAt = DateTime.UtcNow.AddDays(-7) },
            new Announcement { Title = "Welcome to CS101 Section B", Content = "Welcome to the Tuesday/Thursday session.", CreatedByTeacherId = teachers[1].Id, CreatedByTeacherName = "Dr. Johnson", TargetCourseInstanceId = courseInstances[1].Id, PublishedAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow },
            new Announcement { Title = "Data Structures Begins", Content = "Welcome to CS201.", CreatedByTeacherId = teachers[0].Id, CreatedByTeacherName = "Dr. Smith", TargetCourseInstanceId = courseInstances[2].Id, PublishedAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow },
            new Announcement { Title = "Midterm Schedule", Content = "Midterm exam is Dec 15th.", CreatedByTeacherId = teachers[0].Id, CreatedByTeacherName = "Dr. Smith", TargetCourseInstanceId = courseInstances[2].Id, PublishedAt = DateTime.UtcNow.AddDays(-5), CreatedAt = DateTime.UtcNow.AddDays(-5) },
            new Announcement { Title = "Algorithms Overview", Content = "CS301 covers analysis and sorting.", CreatedByTeacherId = teachers[1].Id, CreatedByTeacherName = "Dr. Johnson", TargetCourseInstanceId = courseInstances[3].Id, PublishedAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow },
            new Announcement { Title = "Circuit Lab Safety", Content = "Wear safety goggles.", CreatedByTeacherId = teachers[2].Id, CreatedByTeacherName = "Dr. Brown", TargetCourseInstanceId = courseInstances[6].Id, PublishedAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow },
            new Announcement { Title = "Calculus I Starts", Content = "Welcome to MATH101.", CreatedByTeacherId = teachers[4].Id, CreatedByTeacherName = "Dr. Taylor", TargetCourseInstanceId = courseInstances[8].Id, PublishedAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow },
            new Announcement { Title = "Homework Collection", Content = "Problem set 1 due next week.", CreatedByTeacherId = teachers[4].Id, CreatedByTeacherName = "Dr. Taylor", TargetCourseInstanceId = courseInstances[8].Id, PublishedAt = DateTime.UtcNow.AddDays(-2), CreatedAt = DateTime.UtcNow.AddDays(-2) }
        };
        context.Announcements.AddRange(announcements);
        await context.SaveChangesAsync();

        // 9. Grades
        var grades = new List<Grade>();
        // CS101 Section A
        for (int i = 0; i < 5 && i < cseStudents.Count; i++) { var score = CalculateScore(80 + i * 2, 82 + i * 2, 85 + i * 2, 88 + i * 2); grades.Add(new Grade { StudentId = cseStudents[i].Id, CourseInstanceId = courseInstances[0].Id, SubmittedByTeacherId = teachers[0].Id, Exam1 = 80 + i * 2, Exam2 = 82 + i * 2, Final = 85 + i * 2, Project = 88 + i * 2, Score = score, LetterGrade = CalculateLetterGrade(score), SubmittedAt = DateTime.UtcNow.AddDays(-10), CreatedAt = DateTime.UtcNow.AddDays(-10) }); }
        // CS201
        for (int i = 0; i < 5 && i < cseStudents.Count; i++) { var score = CalculateScore(85 + i, 87 + i, 89 + i, null); grades.Add(new Grade { StudentId = cseStudents[i].Id, CourseInstanceId = courseInstances[2].Id, SubmittedByTeacherId = teachers[0].Id, Exam1 = 85 + i, Exam2 = 87 + i, Final = 89 + i, Project = null, Score = score, LetterGrade = CalculateLetterGrade(score), SubmittedAt = DateTime.UtcNow.AddDays(-8), CreatedAt = DateTime.UtcNow.AddDays(-8) }); }
        // ENG101
        for (int i = 0; i < engStudents.Count; i++) { var score = CalculateScore(75 + i * 2, 77 + i * 2, 80 + i * 2, 82 + i * 2); grades.Add(new Grade { StudentId = engStudents[i].Id, CourseInstanceId = courseInstances[6].Id, SubmittedByTeacherId = teachers[2].Id, Exam1 = 75 + i * 2, Exam2 = 77 + i * 2, Final = 80 + i * 2, Project = 82 + i * 2, Score = score, LetterGrade = CalculateLetterGrade(score), SubmittedAt = DateTime.UtcNow.AddDays(-7), CreatedAt = DateTime.UtcNow.AddDays(-7) }); }
        // MATH101
        for (int i = 0; i < mathStudents.Count; i++) { var score = CalculateScore(78 + i * 2, 80 + i * 2, 82 + i * 2, null); grades.Add(new Grade { StudentId = mathStudents[i].Id, CourseInstanceId = courseInstances[8].Id, SubmittedByTeacherId = teachers[4].Id, Exam1 = 78 + i * 2, Exam2 = 80 + i * 2, Final = 82 + i * 2, Project = null, Score = score, LetterGrade = CalculateLetterGrade(score), SubmittedAt = DateTime.UtcNow.AddDays(-5), CreatedAt = DateTime.UtcNow.AddDays(-5) }); }
        // PHYS101
        for (int i = 0; i < physicsStudents.Count; i++) { var score = CalculateScore(82 + i, 84 + i, 86 + i, 85 + i); grades.Add(new Grade { StudentId = physicsStudents[i].Id, CourseInstanceId = courseInstances[10].Id, SubmittedByTeacherId = teachers[5].Id, Exam1 = 82 + i, Exam2 = 84 + i, Final = 86 + i, Project = 85 + i, Score = score, LetterGrade = CalculateLetterGrade(score), SubmittedAt = DateTime.UtcNow.AddDays(-3), CreatedAt = DateTime.UtcNow.AddDays(-3) }); }

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

        // ==================================================================================
        // 11. Assignments (MIGRATED CODE)
        // ==================================================================================

        // We use the objects created above (courseInstances, teachers) to ensure IDs match.

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
                new Assignment
                {
                    CourseInstanceId = courseInstances[0].Id,
                    CreatedByTeacherId = teachers[0].Id,
                    Title = "Functions and Methods",
                    Description = "Create a calculator program with separate methods for add, subtract, multiply, and divide operations.",
                    DueDate = DateTime.UtcNow.AddDays(21),
                    TotalPoints = 120,
                    Status = AssignmentStatus.Published,
                    CreatedAt = DateTime.UtcNow.AddDays(-5)
                },

                // CS101 Section B (Johnson)
                new Assignment
                {
                    CourseInstanceId = courseInstances[1].Id,
                    CreatedByTeacherId = teachers[1].Id,
                    Title = "Introduction to Programming",
                    Description = "Write your first Hello World program and explain the basic structure of a C# program.",
                    DueDate = DateTime.UtcNow.AddDays(5),
                    TotalPoints = 50,
                    Status = AssignmentStatus.Published,
                    CreatedAt = DateTime.UtcNow.AddDays(-12)
                },
                new Assignment
                {
                    CourseInstanceId = courseInstances[1].Id,
                    CreatedByTeacherId = teachers[1].Id,
                    Title = "Arrays and Collections",
                    Description = "Demonstrate the use of arrays and List<T> collections. Create a program that manages a list of student names.",
                    DueDate = DateTime.UtcNow.AddDays(10),
                    TotalPoints = 100,
                    Status = AssignmentStatus.Published,
                    CreatedAt = DateTime.UtcNow.AddDays(-8)
                },

                // MATH101 (Taylor)
                new Assignment
                {
                    CourseInstanceId = courseInstances[8].Id,
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
                    CourseInstanceId = courseInstances[8].Id,
                    CreatedByTeacherId = teachers[4].Id,
                    Title = "Derivatives Practice",
                    Description = "Calculate derivatives for 20 different functions using various differentiation rules.",
                    DueDate = DateTime.UtcNow.AddDays(12),
                    TotalPoints = 150,
                    Status = AssignmentStatus.Published,
                    CreatedAt = DateTime.UtcNow.AddDays(-9)
                },
                new Assignment
                {
                    CourseInstanceId = courseInstances[8].Id,
                    CreatedByTeacherId = teachers[4].Id,
                    Title = "Integration Techniques",
                    Description = "Apply integration techniques to solve 15 integral problems. Include substitution and integration by parts.",
                    DueDate = DateTime.UtcNow.AddDays(18),
                    TotalPoints = 120,
                    Status = AssignmentStatus.Published,
                    CreatedAt = DateTime.UtcNow.AddDays(-6)
                },

                // PHYS101 (Miller)
                new Assignment
                {
                    CourseInstanceId = courseInstances[10].Id,
                    CreatedByTeacherId = teachers[5].Id,
                    Title = "Newton's Laws of Motion",
                    Description = "Solve 10 problems applying Newton's three laws of motion. Include free body diagrams.",
                    DueDate = DateTime.UtcNow.AddDays(8),
                    TotalPoints = 100,
                    Status = AssignmentStatus.Published,
                    CreatedAt = DateTime.UtcNow.AddDays(-13)
                },
                new Assignment
                {
                    CourseInstanceId = courseInstances[10].Id,
                    CreatedByTeacherId = teachers[5].Id,
                    Title = "Energy and Work",
                    Description = "Complete problems on kinetic energy, potential energy, and work-energy theorem.",
                    DueDate = DateTime.UtcNow.AddDays(15),
                    TotalPoints = 130,
                    Status = AssignmentStatus.Published,
                    CreatedAt = DateTime.UtcNow.AddDays(-7)
                },

                // ENG101 (Brown)
                new Assignment
                {
                    CourseInstanceId = courseInstances[6].Id,
                    CreatedByTeacherId = teachers[2].Id,
                    Title = "Essay Writing - Introduction",
                    Description = "Write a 3-page essay on a topic of your choice. Focus on proper thesis statement and paragraph structure.",
                    DueDate = DateTime.UtcNow.AddDays(10),
                    TotalPoints = 100,
                    Status = AssignmentStatus.Published,
                    CreatedAt = DateTime.UtcNow.AddDays(-11)
                },
                new Assignment
                {
                    CourseInstanceId = courseInstances[6].Id,
                    CreatedByTeacherId = teachers[2].Id,
                    Title = "Literary Analysis",
                    Description = "Analyze a short story of your choice. Discuss theme, character development, and literary devices used.",
                    DueDate = DateTime.UtcNow.AddDays(16),
                    TotalPoints = 150,
                    Status = AssignmentStatus.Published,
                    CreatedAt = DateTime.UtcNow.AddDays(-8)
                }
                
                // Note: CHEM101 assignments excluded as there is no CHEM Department/Course in the seed data.
            };

            context.Assignments.AddRange(assignments);
            await context.SaveChangesAsync();

            var submissions = new List<AssignmentSubmission>
            {
                // Ahmed Hassan (cseStudents[0]) submissions
                new AssignmentSubmission
                {
                    AssignmentId = assignments[0].Id, // "Variables and Data Types"
                    StudentId = cseStudents[0].Id,
                    SubmissionText = "I have completed the variables assignment. My program demonstrates int, string, bool, double, and char data types with proper examples and comments.",
                    SubmittedAt = DateTime.UtcNow.AddDays(-3),
                    Grade = 95,
                    GradedAt = DateTime.UtcNow.AddDays(-1),
                    GradedByTeacherId = teachers[0].Id, // Dr. Smith graded it
                    Status = SubmissionStatus.Graded,
                    CreatedAt = DateTime.UtcNow.AddDays(-3)
                },
                new AssignmentSubmission
                {
                    AssignmentId = assignments[1].Id, // "Control Flow"
                    StudentId = cseStudents[0].Id,
                    SubmissionText = "Submitted control flow assignment with all required examples.",
                    SubmittedAt = DateTime.UtcNow.AddDays(-2),
                    Status = SubmissionStatus.Submitted,
                    CreatedAt = DateTime.UtcNow.AddDays(-2)
                },

                // Fatima Khan (cseStudents[1]) submissions
                new AssignmentSubmission
                {
                    AssignmentId = assignments[0].Id, // "Variables and Data Types"
                    StudentId = cseStudents[1].Id,
                    SubmissionText = "Completed assignment on data types. Included examples and explanations for each type.",
                    SubmittedAt = DateTime.UtcNow.AddDays(-4),
                    Grade = 88,
                    GradedAt = DateTime.UtcNow.AddDays(-2),
                    GradedByTeacherId = teachers[0].Id,
                    Status = SubmissionStatus.Graded,
                    CreatedAt = DateTime.UtcNow.AddDays(-4)
                },

                // Ali Yilmaz (mathStudents[0]) submissions for MATH101
                new AssignmentSubmission
                {
                    AssignmentId = assignments[5].Id, // "Limits and Continuity" (index 5 in list above)
                    StudentId = mathStudents[0].Id,
                    SubmissionText = "Solved all 15 problems on limits and continuity with detailed work shown.",
                    SubmittedAt = DateTime.UtcNow.AddDays(-5),
                    Grade = 92,
                    GradedAt = DateTime.UtcNow.AddDays(-3),
                    GradedByTeacherId = teachers[4].Id, // Dr. Taylor
                    Status = SubmissionStatus.Graded,
                    CreatedAt = DateTime.UtcNow.AddDays(-5)
                },

                // Zeynep Demir (physicsStudents[0]) submissions for PHYS101
                new AssignmentSubmission
                {
                    AssignmentId = assignments[8].Id, // "Newton's Laws" (index 8 in list above)
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