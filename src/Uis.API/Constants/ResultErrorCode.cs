using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uis.API.Constants;

public enum ResultErrorCode
{
    ValidationError = 1,
    NotFound = 2,
    Unauthorized = 3,
    Forbidden = 4,
    Conflict = 5,
    TooManyRequests = 6,
    CourseFull = 7,
    ConflictingSchedule = 8,
    PrerequisiteNotMet = 9,
    AlreadyEnrolled = 10,
    InternalServerError = 11
}