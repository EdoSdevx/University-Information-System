using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uis.API.Constants;
using Uis.API.DTOs.AcademicYear;
using Uis.API.Models;
using Uis.API.Repositories.Interfaces;
using Uis.API.Services.Interfaces;

namespace Uis.API.Services;

public class AcademicYearService : IAcademicYearService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AcademicYearService> _logger;

    public AcademicYearService(IUnitOfWork unitOfWork, ILogger<AcademicYearService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task<ResultService<AcademicYearResponse>> GetActiveAcademicYearAsync()
    {
        _logger.LogInformation("Retrieving active academic year");

        var academicYear = await _unitOfWork.AcademicYears.GetActiveAsync();

        if (academicYear == null)
        {
            _logger.LogWarning("No active academic year found");
            return ResultService<AcademicYearResponse>.NotFound("No active academic year exists");
        }

        var dto = MapToAcademicYearResponse(academicYear);
        _logger.LogInformation("Retrieved active academic year: {Year}", academicYear.Year);
        return ResultService<AcademicYearResponse>.Ok(dto, "Active academic year retrieved");
    }
    public async Task<ResultService<AcademicYearResponse>> GetAcademicYearByIdAsync(int id)
    {
        _logger.LogInformation("Retrieving academic year with ID: {AcademicYearId}", id);

        var academicYear = await _unitOfWork.AcademicYears.GetByIdAsync(id);

        if (academicYear == null)
        {
            _logger.LogWarning("Academic year with ID {AcademicYearId} not found", id);
            return ResultService<AcademicYearResponse>.NotFound($"Academic year not found");
        }

        var dto = MapToAcademicYearResponse(academicYear);
        return ResultService<AcademicYearResponse>.Ok(dto, "Academic year retrieved");
    }



    // ==================== MAPPING ====================

    private static AcademicYearResponse MapToAcademicYearResponse(AcademicYear academicYear)
    {
        return new AcademicYearResponse
        {
            Id = academicYear.Id,
            StartYear = academicYear.StartYear,
            EndYear = academicYear.EndYear,
            EnrollmentStartDate = academicYear.EnrollmentStartDate,
            EnrollmentEndDate = academicYear.EnrollmentEndDate,
            IsActive = academicYear.IsActive,
            CreatedAt = academicYear.CreatedAt
        };
    }


}