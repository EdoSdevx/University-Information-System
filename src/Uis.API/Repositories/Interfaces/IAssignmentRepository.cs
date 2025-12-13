using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uis.API.Models;

namespace Uis.API.Repositories.Interfaces;

public interface IAssignmentRepository : IBaseRepository<Assignment>
{
    Task<List<Assignment>> GetTeacherAssignmentsAsync(int teacherId, int courseInstanceId);
    Task<List<Assignment>> GetStudentAssignmentsAsync(int studentId);
    Task<Assignment?> GetAssignmentWithSubmissionsAsync(int assignmentId);
}