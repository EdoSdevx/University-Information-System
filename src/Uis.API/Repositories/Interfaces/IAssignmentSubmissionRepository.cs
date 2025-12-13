using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uis.API.Models;

namespace Uis.API.Repositories.Interfaces;
public interface IAssignmentSubmissionRepository : IBaseRepository<AssignmentSubmission>
{
    Task<AssignmentSubmission?> GetStudentSubmissionAsync(int assignmentId, int studentId);
    Task<List<AssignmentSubmission>> GetAssignmentSubmissionsAsync(int assignmentId);
}