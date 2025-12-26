# 🎓 University Information System

A full-stack university portal built with **ASP.NET Core 9** and **Vanilla JavaScript**, featuring role-based dashboards for Students, Teachers, and Administrators.

![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=flat-square&logo=dotnet)
![SQL Server](https://img.shields.io/badge/SQL%20Server-Database-CC2927?style=flat-square&logo=microsoftsqlserver)
![JavaScript](https://img.shields.io/badge/JavaScript-ES6+-F7DF1E?style=flat-square&logo=javascript&logoColor=black)
![JWT](https://img.shields.io/badge/JWT-Authentication-000000?style=flat-square&logo=jsonwebtokens)

---

## ✨ Features

### 👨‍🎓 Student Portal
- View enrolled courses and weekly schedule
- Track attendance records
- Submit assignments with file uploads
- View grades and GPA calculation
- Course registration system
- Announcements feed

### 👩‍🏫 Teacher Portal
- Manage assigned classes and student rosters
- Bell-curve grading system
- Attendance tracking per week/day
- Create and grade assignments
- Post course announcements

### 👨‍💼 Admin Portal
- User management (CRUD operations)
- Department management
- Course and course instance management
- System-wide configuration

---

## 🏗️ Architecture

```
University-Information-System/
├── src/
│   ├── Uis.API/                    # ASP.NET Core Web API
│   │   ├── Controllers/            # 12 API controllers
│   │   ├── Services/               # Business logic layer
│   │   ├── Repositories/           # Data access layer
│   │   ├── Models/                 # Entity Framework models
│   │   ├── DTOs/                   # Data transfer objects
│   │   └── Validators/             # FluentValidation rules
│   │
│   └── WebApp/                     # Frontend SPA
│       ├── index.html              # Single page application
│       ├── style.css               # Custom CSS (no frameworks)
│       └── js/
│           ├── core/               # API client, router, state
│           ├── components/         # Header, sidebar components
│           └── pages/              # Role-specific page modules
```

---

## 🛠️ Tech Stack

| Layer | Technology |
|-------|------------|
| **Backend** | ASP.NET Core 9, Entity Framework Core |
| **Database** | SQL Server |
| **Authentication** | JWT + Refresh Tokens |
| **Password Security** | BCrypt |
| **Validation** | FluentValidation |
| **API Documentation** | Swagger / OpenAPI |
| **Frontend** | Vanilla JavaScript (ES6 Modules) |
| **Styling** | Custom CSS |

---

## 🔐 Security Features

- ✅ JWT authentication with refresh token rotation
- ✅ BCrypt password hashing
- ✅ Rate limiting on authentication endpoints
- ✅ Role-based authorization (Student, Teacher, Admin)
- ✅ File upload validation (size limits, extension whitelist)
- ✅ Input validation with FluentValidation

---

## 🚀 Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/sql-server) (or SQL Server Express)
- [Node.js](https://nodejs.org/) (for serving the frontend)

### Backend Setup

1. Clone the repository
   ```bash
   git clone https://github.com/EdoSdevx/University-Information-System.git
   cd University-Information-System
   ```

2. Configure the database connection in `src/Uis.API/appsettings.Development.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=UniversityDB;Trusted_Connection=True;TrustServerCertificate=True"
     },
     "JwtSettings": {
       "SecretKey": "your-secret-key-min-32-characters",
       "Issuer": "UniversityAPI",
       "Audience": "UniversityClient",
       "ExpirationMinutes": "60"
     }
   }
   ```

3. Run the API
   ```bash
   cd src/Uis.API
   dotnet run
   ```
   The API will start at `http://localhost:5000` with Swagger UI available at `/swagger`.

### Frontend Setup

1. Serve the WebApp folder (using any static server)
   ```bash
   cd src/WebApp
   npx serve -p 8080
   ```

2. Open `http://127.0.0.1:8080` in your browser

### Default Seed Data
The application seeds sample data on first run including:
- Admin, Teacher, and Student accounts
- Departments and Courses
- Sample enrollments and grades

---

## 📸 Screenshots

### Login Page
![Login Page](https://i.imgur.com/Ehkfa1N.png)

### Student Dashboard
![Student Dashboard](https://i.imgur.com/xElGejs.png)

### Teacher Dashboard
![Teacher Dashboard](https://i.imgur.com/aMZBnmk.png)

### Admin Dashboard
![Admin Dashboard](https://i.imgur.com/q0yZnu7.png)

---

## 📝 API Endpoints

| Controller | Endpoints | Description |
|------------|-----------|-------------|
| Authentication | `/api/authentication/*` | Login, Register, Refresh Token |
| Users | `/api/user/*` | User CRUD operations |
| Courses | `/api/course/*` | Course management |
| CourseInstance | `/api/courseinstance/*` | Semester instances |
| Enrollment | `/api/enrollment/*` | Student enrollments |
| Grade | `/api/grade/*` | Grading system |
| Attendance | `/api/attendance/*` | Attendance tracking |
| Assignment | `/api/assignment/*` | Assignments & submissions |
| Announcement | `/api/announcement/*` | Course announcements |

Full API documentation available via Swagger UI when running the application.

---

## 👤 Author

**EdoSdevx**

- GitHub: [@EdoSdevx](https://github.com/EdoSdevx)
