using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uis.API.DTOs.CourseInstance;

namespace Uis.API.Validators;

public class CreateCourseInstanceRequestValidator : AbstractValidator<CreateCourseInstanceRequest>
{
    public CreateCourseInstanceRequestValidator()
    {
        RuleFor(x => x.CourseId).GreaterThan(0);
        RuleFor(x => x.TeacherId).GreaterThan(0);
        RuleFor(x => x.AcademicYearId).GreaterThan(0);

        RuleFor(x => x.Section)
            .NotEmpty()
            .Matches(@"^\d{1,2}$").WithMessage("Section should be a number (1, 2, 3 etc.)");

        RuleFor(x => x.Capacity).InclusiveBetween(1, 500);

        RuleFor(x => x.StartTime).NotNull().When(x => x.Day1.HasValue || x.Day2.HasValue);
        RuleFor(x => x.EndTime)
            .NotNull().When(x => x.StartTime.HasValue)
            .GreaterThan(x => x.StartTime).When(x => x.StartTime.HasValue)
            .WithMessage("EndTime must be after StartTime");
    }
}