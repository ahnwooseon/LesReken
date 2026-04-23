using FluentResults;
using LesReken.Domain.Models;

namespace LesReken.Application.Interfaces;

public interface IStudentService
{
    Task<Result<Student>> AddAsync(Student student, CancellationToken ct = default);
    Task<Result<Student>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Result<IReadOnlyCollection<Student>>> GetAllAsync(CancellationToken ct = default);
    Task<Result> UpdateAsync(Student student, CancellationToken ct = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken ct = default);
}
