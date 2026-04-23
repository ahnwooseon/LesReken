using FluentResults;
using LesReken.Application.Interfaces;
using LesReken.Domain.Models;

namespace LesReken.Application.Services;

public class StudentService(IStudentRepository repository) : IStudentService
{
    public async Task<Result<Student>> AddAsync(Student student, CancellationToken ct = default)
    {
        Student created = await repository.AddAsync(student, ct);
        return Result.Ok(created);
    }

    public async Task<Result<Student>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        Student? student = await repository.GetByIdAsync(id, ct);
        return student is null ? Result.Fail("L'élève n'a pas été trouvé.") : Result.Ok(student);
    }

    public async Task<Result<IReadOnlyCollection<Student>>> GetAllAsync(
        CancellationToken ct = default
    )
    {
        IReadOnlyCollection<Student> students = await repository.GetAllAsync(ct);
        return Result.Ok(students);
    }

    public async Task<Result> UpdateAsync(Student student, CancellationToken ct = default)
    {
        bool success = await repository.UpdateAsync(student, ct);
        return success ? Result.Ok() : Result.Fail("La mise à jour a échoué (élève introuvable).");
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        bool success = await repository.DeleteAsync(id, ct);
        return success ? Result.Ok() : Result.Fail("La suppression a échoué (élève introuvable).");
    }
}
