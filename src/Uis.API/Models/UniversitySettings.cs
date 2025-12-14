using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uis.API.Models;
public class UniversitySettings
{
    public string StudentAffairsEmail { get; set; } = string.Empty;
    public string RegistrarEmail { get; set; } = string.Empty;
    public string ITSupportEmail { get; set; } = string.Empty;
    public string AcademicAffairsEmail { get; set; } = string.Empty;
    public string HREmail { get; set; } = string.Empty;
    public string CounselingEmail { get; set; } = string.Empty;
}
