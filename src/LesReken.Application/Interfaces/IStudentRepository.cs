using LesReken.Domain.Models;

namespace LesReken.Application.Interfaces;

public interface IStudentRepository
{
    Task<Student> AddAsync(Student student, CancellationToken ct = default);
    Task<Student?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyCollection<Student>> GetAllAsync(CancellationToken ct = default);
    Task<bool> UpdateAsync(Student student, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
}
