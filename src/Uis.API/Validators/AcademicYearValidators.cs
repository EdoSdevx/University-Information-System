using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uis.API.DTOs.AcademicYear;

namespace Uis.API.Validators;

public class CreateAcademicYearRequestValidator : AbstractValidator<AcademicYearRequest>
{
    public CreateAcademicYearRequestValidator()
    {

        RuleFor(x => x.StartYear).GreaterThan(2000).LessThan(2100);
        RuleFor(x => x.EndYear).Equal(x => x.StartYear + 1).WithMessage("EndYear must be StartYear + 1");

        RuleFor(x => x.EnrollmentStartDate).NotEmpty();
        RuleFor(x => x.EnrollmentEndDate)
            .NotEmpty()
            .GreaterThan(x => x.EnrollmentStartDate)
            .WithMessage("EnrollmentEndDate must be after StartDate");
    }
}