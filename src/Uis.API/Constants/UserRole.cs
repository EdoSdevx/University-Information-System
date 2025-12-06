using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uis.API.Constants;

public enum UserRole
{
    Admin,
    Student,
    Teacher
}

public static class Roles
{
    public const string Admin = nameof(UserRole.Admin);
    public const string Student = nameof(UserRole.Student);
    public const string Teacher = nameof(UserRole.Teacher);
}