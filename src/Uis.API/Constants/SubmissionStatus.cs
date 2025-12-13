using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uis.API.Constants;
public enum SubmissionStatus
{
    Pending = 0,
    Submitted = 1,
    Graded = 2,
    Late = 3,
    Rejected = 4
}
