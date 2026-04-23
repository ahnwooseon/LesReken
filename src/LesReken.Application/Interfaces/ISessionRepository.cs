using LesReken.Domain.Models;

namespace LesReken.Application.Interfaces;

public interface ISessionRepository
{
    Task<Session> AddAsync(Session session, CancellationToken ct = default);
    Task<Session?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyCollection<Session>> GetAllAsync(CancellationToken ct = default);
    Task<bool> UpdateAsync(Session session, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
}
