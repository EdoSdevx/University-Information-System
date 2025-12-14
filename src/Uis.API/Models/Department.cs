using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uis.API.Models;

public class Department : BaseEntity
{
    public required string Name { get; set; }
    public required string Code { get; set; }
    public string Email { get; set; }
    public string SecretaryEmail { get; set; }
    public string DepartmentHeadName { get; set; }
    public string DepartmentHeadEmail { get; set; }
    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}

