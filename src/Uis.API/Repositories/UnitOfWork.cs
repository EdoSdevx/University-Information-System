using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uis.API.Models;
using Uis.API.Repositories.Interfaces;

namespace Uis.API.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;

    private IUserRepository? _users;
    private IDepartmentRepository? _departments;
    private ICourseRepository? _courses;
    private ICourseInstanceRepository? _courseInstances;
    private IAcademicYearRepository? _academicYears;
    private IEnrollmentRepository? _enrollments;
    private IGradeRepository? _grades;
    private IAnnouncementRepository? _announcements;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IUserRepository Users => _users ??= new UserRepository(_context);

    public IDepartmentRepository Departments => _departments ??= new DepartmentRepository(_context);

    public ICourseRepository Courses => _courses ??= new CourseRepository(_context);

    public ICourseInstanceRepository CourseInstances => _courseInstances ??= new CourseInstanceRepository(_context);

    public IAcademicYearRepository AcademicYears => _academicYears ??= new AcademicYearRepository(_context);

    public IEnrollmentRepository Enrollments => _enrollments ??= new EnrollmentRepository(_context);

    public IGradeRepository Grades => _grades ??= new GradeRepository(_context);

    public IAnnouncementRepository Announcements => _announcements ??= new AnnouncementRepository(_context, _enrollments);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task<IAsyncDisposable> BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
        return _transaction;
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync()
    {
        try
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
            }
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_transaction != null)
        {
            await _transaction.DisposeAsync();
        }
        if (_context != null)
        {
            await _context.DisposeAsync();
        }
    }

}
