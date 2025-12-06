using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uis.API.DTOs.Course;

namespace Uis.API.Validators;
public class CreateCourseRequestValidator : AbstractValidator<CreateCourseRequest>
{
    public CreateCourseRequestValidator()
    {
        RuleFor(x => x.Code).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.CreditHours).InclusiveBetween(1, 10);
        RuleFor(x => x.DepartmentId).GreaterThan(0);
        RuleFor(x => x.PrerequisiteCourseId)
            .GreaterThan(0).When(x => x.PrerequisiteCourseId.HasValue);
    }
}
