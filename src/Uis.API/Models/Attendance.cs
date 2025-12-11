using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uis.API.Constants;

namespace Uis.API.Models;

public class Attendance : BaseEntity
{
    public int EnrollmentId { get; set; }
    public int Week { get; set; }
    public DayOfWeek? Day { get; set; }
    public string Status { get; set; } = string.Empty;

    public Enrollment? Enrollment { get; set; }
}


