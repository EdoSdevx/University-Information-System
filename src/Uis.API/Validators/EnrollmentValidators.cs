using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uis.API.DTOs.Enrollment;


namespace Uis.API.Validators;
public class EnrollStudentRequestValidator : AbstractValidator<EnrollStudentRequest>
{
    public EnrollStudentRequestValidator()
    {
        RuleFor(x => x.CourseInstanceId).GreaterThan(0);
    }
}