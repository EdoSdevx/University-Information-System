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
        if (context.Users.Any())
        {
            return;
        }

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

        var departments = new List<Department>
        {
            new Department { Code = "CSE", Name = "Computer Science & Engineering", CreatedAt = DateTime.UtcNow },
            new Department { Code = "ENG", Name = "Electrical Engineering", CreatedAt = DateTime.UtcNow },
            new Department { Code = "MATH", Name = "Mathematics", CreatedAt = DateTime.UtcNow },
            new Department { Code = "PHYS", Name = "Physics", CreatedAt = DateTime.UtcNow }
        };
        context.Departments.AddRange(departments);
        await context.SaveChangesAsync();

        var courses = new List<Course>
        {
            new Course { Code = "CS101", Name = "Introduction to Computer Science", CreditHours = 3, DepartmentId = departments[0].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "CS201", Name = "Data Structures", CreditHours = 4, DepartmentId = departments[0].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "CS301", Name = "Algorithms", CreditHours = 4, DepartmentId = departments[0].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "CS401", Name = "Database Systems", CreditHours = 3, DepartmentId = departments[0].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "CS501", Name = "Web Development", CreditHours = 3, DepartmentId = departments[0].Id, CreatedAt = DateTime.UtcNow },

            new Course { Code = "ENG101", Name = "Circuit Analysis", CreditHours = 4, DepartmentId = departments[1].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "ENG201", Name = "Digital Logic Design", CreditHours = 4, DepartmentId = departments[1].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "ENG301", Name = "Signal Processing", CreditHours = 3, DepartmentId = departments[1].Id, CreatedAt = DateTime.UtcNow },

            new Course { Code = "MATH101", Name = "Calculus I", CreditHours = 4, DepartmentId = departments[2].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "MATH201", Name = "Linear Algebra", CreditHours = 3, DepartmentId = departments[2].Id, CreatedAt = DateTime.UtcNow },

            new Course { Code = "PHYS101", Name = "Physics I", CreditHours = 4, DepartmentId = departments[3].Id, CreatedAt = DateTime.UtcNow },
            new Course { Code = "PHYS201", Name = "Physics II", CreditHours = 4, DepartmentId = departments[3].Id, CreatedAt = DateTime.UtcNow }
        };
        context.Courses.AddRange(courses);
        await context.SaveChangesAsync();

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
            CreatedAt = DateTime.UtcNow
        }).ToList();

        context.Users.Add(admin);
        context.Users.AddRange(teachers);
        context.Users.AddRange(cseStudents);
        context.Users.AddRange(engStudents);
        context.Users.AddRange(mathStudents);
        context.Users.AddRange(physicsStudents);
        await context.SaveChangesAsync();

        var courseInstances = new List<CourseInstance>
        {
            new CourseInstance
            {
                CourseId = courses[0].Id, TeacherId = teachers[0].Id, AcademicYearId = academicYear.Id,
                DepartmentId = departments[0].Id, Section = "A", Capacity = 30, CurrentEnrollmentCount = 0,
                Day1 = DayOfWeek.Monday, Day2 = DayOfWeek.Wednesday, StartTime = new TimeOnly(9, 0),
                EndTime = new TimeOnly(10, 30), Location = "Room 101", Status = CourseInstanceStatus.Open,
                ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow
            },
            new CourseInstance
            {
                CourseId = courses[0].Id, TeacherId = teachers[1].Id, AcademicYearId = academicYear.Id,
                DepartmentId = departments[0].Id, Section = "B", Capacity = 25, CurrentEnrollmentCount = 0,
                Day1 = DayOfWeek.Tuesday, Day2 = DayOfWeek.Thursday, StartTime = new TimeOnly(11, 0),
                EndTime = new TimeOnly(12, 30), Location = "Room 102", Status = CourseInstanceStatus.Open,
                ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow
            },
            new CourseInstance
            {
                CourseId = courses[1].Id, TeacherId = teachers[0].Id, AcademicYearId = academicYear.Id,
                DepartmentId = departments[0].Id, Section = "A", Capacity = 28, CurrentEnrollmentCount = 0,
                Day1 = DayOfWeek.Monday, Day2 = DayOfWeek.Wednesday, StartTime = new TimeOnly(14, 0),
                EndTime = new TimeOnly(15, 30), Location = "Room 201", Status = CourseInstanceStatus.Open,
                ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow
            },
            new CourseInstance
            {
                CourseId = courses[2].Id, TeacherId = teachers[1].Id, AcademicYearId = academicYear.Id,
                DepartmentId = departments[0].Id, Section = "A", Capacity = 25, CurrentEnrollmentCount = 0,
                Day1 = DayOfWeek.Tuesday, Day2 = DayOfWeek.Thursday, StartTime = new TimeOnly(14, 0),
                EndTime = new TimeOnly(15, 30), Location = "Room 301", Status = CourseInstanceStatus.Open,
                ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow
            },
            new CourseInstance
            {
                CourseId = courses[3].Id, TeacherId = teachers[0].Id, AcademicYearId = academicYear.Id,
                DepartmentId = departments[0].Id, Section = "A", Capacity = 20, CurrentEnrollmentCount = 0,
                Day1 = DayOfWeek.Monday, Day2 = DayOfWeek.Wednesday, StartTime = new TimeOnly(16, 0),
                EndTime = new TimeOnly(17, 30), Location = "Room 401", Status = CourseInstanceStatus.Open,
                ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow
            },
            new CourseInstance
            {
                CourseId = courses[4].Id, TeacherId = teachers[1].Id, AcademicYearId = academicYear.Id,
                DepartmentId = departments[0].Id, Section = "A", Capacity = 5, CurrentEnrollmentCount = 0,
                Day1 = DayOfWeek.Tuesday, Day2 = DayOfWeek.Thursday, StartTime = new TimeOnly(11, 0),
                EndTime = new TimeOnly(12, 30), Location = "Room 501", Status = CourseInstanceStatus.Open,
                ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow
            },

            new CourseInstance
            {
                CourseId = courses[5].Id, TeacherId = teachers[2].Id, AcademicYearId = academicYear.Id,
                DepartmentId = departments[1].Id, Section = "A", Capacity = 30, CurrentEnrollmentCount = 0,
                Day1 = DayOfWeek.Monday, Day2 = DayOfWeek.Wednesday, StartTime = new TimeOnly(9, 0),
                EndTime = new TimeOnly(10, 30), Location = "Lab 101", Status = CourseInstanceStatus.Open,
                ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow
            },
            new CourseInstance
            {
                CourseId = courses[6].Id, TeacherId = teachers[3].Id, AcademicYearId = academicYear.Id,
                DepartmentId = departments[1].Id, Section = "A", Capacity = 25, CurrentEnrollmentCount = 0,
                Day1 = DayOfWeek.Tuesday, Day2 = DayOfWeek.Thursday, StartTime = new TimeOnly(11, 0),
                EndTime = new TimeOnly(12, 30), Location = "Lab 201", Status = CourseInstanceStatus.Open,
                ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow
            },

            new CourseInstance
            {
                CourseId = courses[8].Id, TeacherId = teachers[4].Id, AcademicYearId = academicYear.Id,
                DepartmentId = departments[2].Id, Section = "A", Capacity = 35, CurrentEnrollmentCount = 0,
                Day1 = DayOfWeek.Monday, Day2 = DayOfWeek.Wednesday, StartTime = new TimeOnly(11, 0),
                EndTime = new TimeOnly(12, 30), Location = "Room 501", Status = CourseInstanceStatus.Open,
                ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow
            },
            new CourseInstance
            {
                CourseId = courses[9].Id, TeacherId = teachers[4].Id, AcademicYearId = academicYear.Id,
                DepartmentId = departments[2].Id, Section = "A", Capacity = 25, CurrentEnrollmentCount = 0,
                Day1 = DayOfWeek.Tuesday, Day2 = DayOfWeek.Thursday, StartTime = new TimeOnly(14, 0),
                EndTime = new TimeOnly(15, 30), Location = "Room 601", Status = CourseInstanceStatus.Open,
                ConcurrencyToken = new byte[8], CreatedAt = DateTime.UtcNow
            },

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

        var enrollments = new List<Enrollment>();

        // CSE Students enrolling in multiple courses
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
                enrollments.Add(new Enrollment
                {
                    StudentId = cseStudents[i].Id,
                    CourseInstanceId = courseInstances[2].Id,
                    AcademicYearId = academicYear.Id,
                    Status = EnrollmentStatus.Active,
                    EnrolledAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                });
            }
        }

        // ENG Students
        for (int i = 0; i < engStudents.Count; i++)
        {
            enrollments.Add(new Enrollment
            {
                StudentId = engStudents[i].Id,
                CourseInstanceId = courseInstances[6].Id,
                AcademicYearId = academicYear.Id,
                Status = EnrollmentStatus.Active,
                EnrolledAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            });
        }

        // MATH Students
        for (int i = 0; i < mathStudents.Count; i++)
        {
            enrollments.Add(new Enrollment
            {
                StudentId = mathStudents[i].Id,
                CourseInstanceId = courseInstances[8].Id,
                AcademicYearId = academicYear.Id,
                Status = EnrollmentStatus.Active,
                EnrolledAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            });
        }

        // Physics Students
        for (int i = 0; i < physicsStudents.Count; i++)
        {
            enrollments.Add(new Enrollment
            {
                StudentId = physicsStudents[i].Id,
                CourseInstanceId = courseInstances[10].Id,
                AcademicYearId = academicYear.Id,
                Status = EnrollmentStatus.Active,
                EnrolledAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            });
        }

        context.Enrollments.AddRange(enrollments);

        // Update enrollment counts
        foreach (var ci in courseInstances)
        {
            ci.CurrentEnrollmentCount = enrollments.Count(e => e.CourseInstanceId == ci.Id);
        }

        await context.SaveChangesAsync();

        // Announcements
        var announcements = new List<Announcement>
        {
            new Announcement
            {
                Title = "Welcome to CS101 Section A",
                Content = "Welcome to Introduction to Computer Science. This course covers fundamental programming concepts.",
                CreatedByTeacherId = teachers[0].Id,
                CreatedByTeacherName = "Dr. Smith",
                TargetCourseInstanceId = courseInstances[0].Id,
                PublishedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            },
            new Announcement
            {
                Title = "Assignment 1: Hello World Program",
                Content = "Your first assignment is to write a simple Hello World program in Python. Due next Friday.",
                CreatedByTeacherId = teachers[0].Id,
                CreatedByTeacherName = "Dr. Smith",
                TargetCourseInstanceId = courseInstances[0].Id,
                PublishedAt = DateTime.UtcNow.AddDays(-3),
                CreatedAt = DateTime.UtcNow.AddDays(-3)
            },
            new Announcement
            {
                Title = "Quiz 1 Results",
                Content = "Quiz 1 has been graded. Check your scores on the portal.",
                CreatedByTeacherId = teachers[0].Id,
                CreatedByTeacherName = "Dr. Smith",
                TargetCourseInstanceId = courseInstances[0].Id,
                PublishedAt = DateTime.UtcNow.AddDays(-7),
                CreatedAt = DateTime.UtcNow.AddDays(-7)
            },
            new Announcement
            {
                Title = "Welcome to CS101 Section B",
                Content = "Welcome to the Tuesday/Thursday session of CS101. Looking forward to meeting you all.",
                CreatedByTeacherId = teachers[1].Id,
                CreatedByTeacherName = "Dr. Johnson",
                TargetCourseInstanceId = courseInstances[1].Id,
                PublishedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            },
            new Announcement
            {
                Title = "Data Structures Course Begins",
                Content = "Welcome to CS201. This course will cover arrays, linked lists, stacks, queues, and trees.",
                CreatedByTeacherId = teachers[0].Id,
                CreatedByTeacherName = "Dr. Smith",
                TargetCourseInstanceId = courseInstances[2].Id,
                PublishedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            },
            new Announcement
            {
                Title = "Midterm Exam Schedule",
                Content = "Midterm exam will be held on December 15th from 2:00 PM to 3:30 PM in Room 101.",
                CreatedByTeacherId = teachers[0].Id,
                CreatedByTeacherName = "Dr. Smith",
                TargetCourseInstanceId = courseInstances[2].Id,
                PublishedAt = DateTime.UtcNow.AddDays(-5),
                CreatedAt = DateTime.UtcNow.AddDays(-5)
            },
            new Announcement
            {
                Title = "Algorithms Course Overview",
                Content = "CS301 covers algorithm analysis, sorting, searching, graph algorithms, and dynamic programming.",
                CreatedByTeacherId = teachers[1].Id,
                CreatedByTeacherName = "Dr. Johnson",
                TargetCourseInstanceId = courseInstances[3].Id,
                PublishedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            },
            new Announcement
            {
                Title = "Circuit Lab Safety",
                Content = "Important: Always wear safety goggles in the lab. Follow all lab procedures strictly.",
                CreatedByTeacherId = teachers[2].Id,
                CreatedByTeacherName = "Dr. Brown",
                TargetCourseInstanceId = courseInstances[6].Id,
                PublishedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            },
            new Announcement
            {
                Title = "Calculus I Course Starts",
                Content = "Welcome to MATH101. We will cover limits, derivatives, integration, and applications.",
                CreatedByTeacherId = teachers[4].Id,
                CreatedByTeacherName = "Dr. Taylor",
                TargetCourseInstanceId = courseInstances[8].Id,
                PublishedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            },
            new Announcement
            {
                Title = "Calculus Homework Collection",
                Content = "Problem set 1 is due next week. Solutions will be posted after the deadline.",
                CreatedByTeacherId = teachers[4].Id,
                CreatedByTeacherName = "Dr. Taylor",
                TargetCourseInstanceId = courseInstances[8].Id,
                PublishedAt = DateTime.UtcNow.AddDays(-2),
                CreatedAt = DateTime.UtcNow.AddDays(-2)
            }
        };
        context.Announcements.AddRange(announcements);
        await context.SaveChangesAsync();

        // Grades with Exam1, Exam2, Final, Project components
        var grades = new List<Grade>();

        // CS101 Section A grades (cseStudents[0-4])
        for (int i = 0; i < 5 && i < cseStudents.Count; i++)
        {
            var exam1 = 80 + (i * 2);
            var exam2 = 82 + (i * 2);
            var final = 85 + (i * 2);
            var project = 88 + (i * 2);
            var score = CalculateScore(exam1, exam2, final, project);

            grades.Add(new Grade
            {
                StudentId = cseStudents[i].Id,
                CourseInstanceId = courseInstances[0].Id,
                SubmittedByTeacherId = teachers[0].Id,
                Exam1 = exam1,
                Exam2 = exam2,
                Final = final,
                Project = project,
                Score = score,
                LetterGrade = CalculateLetterGrade(score),
                SubmittedAt = DateTime.UtcNow.AddDays(-10),
                CreatedAt = DateTime.UtcNow.AddDays(-10)
            });
        }

        // CS201 Data Structures grades (cseStudents[0-4])
        for (int i = 0; i < 5 && i < cseStudents.Count; i++)
        {
            var exam1 = 85 + (i * 1);
            var exam2 = 87 + (i * 1);
            var final = 89 + (i * 1);
            var project = (int?)null;
            var score = CalculateScore(exam1, exam2, final, project);

            grades.Add(new Grade
            {
                StudentId = cseStudents[i].Id,
                CourseInstanceId = courseInstances[2].Id,
                SubmittedByTeacherId = teachers[0].Id,
                Exam1 = exam1,
                Exam2 = exam2,
                Final = final,
                Project = project,
                Score = score,
                LetterGrade = CalculateLetterGrade(score),
                SubmittedAt = DateTime.UtcNow.AddDays(-8),
                CreatedAt = DateTime.UtcNow.AddDays(-8)
            });
        }

        // Circuit Analysis grades for ENG students
        for (int i = 0; i < engStudents.Count; i++)
        {
            var exam1 = 75 + (i * 2);
            var exam2 = 77 + (i * 2);
            var final = 80 + (i * 2);
            var project = 82 + (i * 2);
            var score = CalculateScore(exam1, exam2, final, project);

            grades.Add(new Grade
            {
                StudentId = engStudents[i].Id,
                CourseInstanceId = courseInstances[6].Id,
                SubmittedByTeacherId = teachers[2].Id,
                Exam1 = exam1,
                Exam2 = exam2,
                Final = final,
                Project = project,
                Score = score,
                LetterGrade = CalculateLetterGrade(score),
                SubmittedAt = DateTime.UtcNow.AddDays(-7),
                CreatedAt = DateTime.UtcNow.AddDays(-7)
            });
        }

        // Calculus I grades for MATH students
        for (int i = 0; i < mathStudents.Count; i++)
        {
            var exam1 = 78 + (i * 2);
            var exam2 = 80 + (i * 2);
            var final = 82 + (i * 2);
            var project = (int?)null;
            var score = CalculateScore(exam1, exam2, final, project);

            grades.Add(new Grade
            {
                StudentId = mathStudents[i].Id,
                CourseInstanceId = courseInstances[8].Id,
                SubmittedByTeacherId = teachers[4].Id,
                Exam1 = exam1,
                Exam2 = exam2,
                Final = final,
                Project = project,
                Score = score,
                LetterGrade = CalculateLetterGrade(score),
                SubmittedAt = DateTime.UtcNow.AddDays(-5),
                CreatedAt = DateTime.UtcNow.AddDays(-5)
            });
        }

        // Physics I grades for Physics students
        for (int i = 0; i < physicsStudents.Count; i++)
        {
            var exam1 = 82 + (i * 1);
            var exam2 = 84 + (i * 1);
            var final = 86 + (i * 1);
            var project = 85 + (i * 1);
            var score = CalculateScore(exam1, exam2, final, project);

            grades.Add(new Grade
            {
                StudentId = physicsStudents[i].Id,
                CourseInstanceId = courseInstances[10].Id,
                SubmittedByTeacherId = teachers[5].Id,
                Exam1 = exam1,
                Exam2 = exam2,
                Final = final,
                Project = project,
                Score = score,
                LetterGrade = CalculateLetterGrade(score),
                SubmittedAt = DateTime.UtcNow.AddDays(-3),
                CreatedAt = DateTime.UtcNow.AddDays(-3)
            });
        }

        context.Grades.AddRange(grades);
        await context.SaveChangesAsync();

        // Attendances
        // Attendances
        var attendances = new List<Attendance>();
        var random = new Random();

        // Create attendance records for each enrollment (weeks 1-12)
        foreach (var enrollment in enrollments)
        {
            var courseInstance = courseInstances.FirstOrDefault(ci => ci.Id == enrollment.CourseInstanceId);
            if (courseInstance == null) continue;

            for (int week = 1; week <= 12; week++)
            {
                // Create record for Day1
                var day1Status = random.Next(0, 100) switch
                {
                    >= 90 => "Absent",         // 10% chance Absent
                    _ => "Present"             // 90% chance Present
                };

                attendances.Add(new Attendance
                {
                    EnrollmentId = enrollment.Id,
                    Week = week,
                    Day = courseInstance.Day1,
                    Status = day1Status,
                    CreatedAt = DateTime.UtcNow
                });

                // Create record for Day2 if it exists
                if (courseInstance.Day2.HasValue)
                {
                    var day2Status = random.Next(0, 100) switch
                    {
                        >= 90 => "Absent",
                        _ => "Present"
                    };

                    attendances.Add(new Attendance
                    {
                        EnrollmentId = enrollment.Id,
                        Week = week,
                        Day = courseInstance.Day2.Value,
                        Status = day2Status,
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }
        }

        context.Attendances.AddRange(attendances);
        await context.SaveChangesAsync();
    }

    private static decimal CalculateScore(int? exam1, int? exam2, int? final, int? project)
    {
        decimal score = 0;
        int componentCount = 0;

        if (exam1.HasValue)
        {
            score += exam1.Value * 0.2m;
            componentCount++;
        }
        if (exam2.HasValue)
        {
            score += exam2.Value * 0.2m;
            componentCount++;
        }
        if (final.HasValue)
        {
            score += final.Value * 0.4m;
            componentCount++;
        }
        if (project.HasValue)
        {
            score += project.Value * 0.2m;
            componentCount++;
        }

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