using FluentResults;
using LesReken.Domain.Models;

namespace LesReken.Application.Interfaces;

public interface ISessionService
{
    Task<Result<Session>> StartSessionAsync(
        decimal hourlyRate,
        List<Guid> studentIds,
        CancellationToken ct = default
    );
    Task<Result<Session>> EndSessionAsync(Guid sessionId, CancellationToken ct = default);
    Task<Result<Session>> UpdateSessionAsync(
        Guid sessionId,
        DateTimeOffset startAt,
        DateTimeOffset? endAt,
        decimal hourlyRate,
        List<Guid> studentIds,
        CancellationToken ct = default
    );
    Task<Result<IReadOnlyCollection<Session>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<Session>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Result> DeleteSessionAsync(Guid sessionId, CancellationToken ct = default);
}
