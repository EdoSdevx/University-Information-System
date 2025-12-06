using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uis.API.DTOs.AcademicYear;

namespace Uis.API.Services.Interfaces;

public interface IAcademicYearService
{
    Task<ResultService<AcademicYearResponse>> GetActiveAcademicYearAsync();
    Task<ResultService<AcademicYearResponse>> GetAcademicYearByIdAsync(int id);
}