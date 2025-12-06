using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uis.API.DTOs.Grade;

namespace Uis.API.Validators;
public class AssignGradeRequestValidator : AbstractValidator<AssignGradeRequest>
{
    public AssignGradeRequestValidator()
    {
        RuleFor(x => x.StudentId).GreaterThan(0);
        RuleFor(x => x.CourseInstanceId).GreaterThan(0);
        RuleFor(x => x.Score).InclusiveBetween(0, 100);
        RuleFor(x => x.LetterGrade).NotEmpty().Length(1, 2).Must(BeValidGrade).WithMessage("Invalid letter grade");
    }

    private bool BeValidGrade(string grade)
    {
        var validGrades = new[] { "AA", "BA", "BB", "CB", "CC", "DC", "DD", "FD", "FF", "F0" };
        return validGrades.Contains(grade.ToUpper());
    }
}
