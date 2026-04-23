using FluentResults;
using LesReken.Application.Interfaces;
using LesReken.Domain.Models;

namespace LesReken.Application.Services;

public class SessionService(ISessionRepository repository, IStudentRepository studentRepository)
    : ISessionService
{
    public async Task<Result<Session>> StartSessionAsync(
        decimal hourlyRate,
        List<Guid> studentIds,
        CancellationToken ct = default
    )
    {
        if (studentIds == null || !studentIds.Any())
            return Result.Fail("Au moins un élève est requis pour démarrer une session.");

        if (hourlyRate <= 0)
            return Result.Fail("Le tarif horaire doit être positif.");

        var session = new Session { StartAt = DateTimeOffset.UtcNow, HourlyRate = hourlyRate };

        foreach (var studentId in studentIds)
        {
            // Vérifier si l'élève existe (optionnel, mais recommandé)
            var student = await studentRepository.GetByIdAsync(studentId, ct);
            if (student != null)
            {
                session.SessionStudents.Add(
                    new SessionStudent { SessionId = session.Id, StudentId = studentId }
                );
            }
        }

        if (!session.SessionStudents.Any())
            return Result.Fail("Aucun des élèves sélectionnés n'a été trouvé.");

        await repository.AddAsync(session, ct);
        return Result.Ok(session);
    }

    public async Task<Result<Session>> EndSessionAsync(
        Guid sessionId,
        CancellationToken ct = default
    )
    {
        var session = await repository.GetByIdAsync(sessionId, ct);
        if (session == null)
            return Result.Fail("La session n'a pas été trouvée.");

        if (session.EndAt.HasValue)
            return Result.Fail("La session est déjà terminée.");

        session.EndAt = DateTimeOffset.UtcNow;
        await repository.UpdateAsync(session, ct);
        return Result.Ok(session);
    }

    public async Task<Result<Session>> UpdateSessionAsync(
        Guid sessionId,
        DateTimeOffset startAt,
        DateTimeOffset? endAt,
        decimal hourlyRate,
        List<Guid> studentIds,
        CancellationToken ct = default
    )
    {
        var session = await repository.GetByIdAsync(sessionId, ct);
        if (session is null)
            return Result.Fail("Session introuvable.");

        if (studentIds is null || !studentIds.Any())
            return Result.Fail("Au moins un élève est requis.");

        if (hourlyRate <= 0)
            return Result.Fail("Le tarif horaire doit être positif.");

        session.StartAt = startAt;
        session.EndAt = endAt;
        session.HourlyRate = hourlyRate;

        // Replace students
        session.SessionStudents.Clear();
        foreach (var studentId in studentIds)
        {
            var student = await studentRepository.GetByIdAsync(studentId, ct);
            if (student is not null)
                session.SessionStudents.Add(
                    new SessionStudent { SessionId = session.Id, StudentId = studentId }
                );
        }

        await repository.UpdateAsync(session, ct);

        // Reload to get the full graph
        var updated = await repository.GetByIdAsync(sessionId, ct);
        return Result.Ok(updated!);
    }

    public async Task<Result<IReadOnlyCollection<Session>>> GetAllAsync(
        CancellationToken ct = default
    )
    {
        var sessions = await repository.GetAllAsync(ct);
        return Result.Ok(sessions);
    }

    public async Task<Result<Session>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var session = await repository.GetByIdAsync(id, ct);
        return session == null ? Result.Fail("Session introuvable.") : Result.Ok(session);
    }

    public async Task<Result> DeleteSessionAsync(Guid sessionId, CancellationToken ct = default)
    {
        var deleted = await repository.DeleteAsync(sessionId, ct);
        return deleted ? Result.Ok() : Result.Fail("La session n'a pas pu être supprimée.");
    }
}
