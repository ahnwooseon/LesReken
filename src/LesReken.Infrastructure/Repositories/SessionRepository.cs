using LesReken.Application.Interfaces;
using LesReken.Domain.Models;
using LesReken.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LesReken.Infrastructure.Repositories;

public class SessionRepository(ApplicationDbContext context) : ISessionRepository
{
    public async Task<Session> AddAsync(Session session, CancellationToken ct = default)
    {
        await context.Sessions.AddAsync(session, ct);
        await context.SaveChangesAsync(ct);
        return session;
    }

    public async Task<Session?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await context
            .Sessions.Include(s => s.SessionStudents)
                .ThenInclude(ss => ss.Student)
            .FirstOrDefaultAsync(s => s.Id == id, ct);

    public async Task<IReadOnlyCollection<Session>> GetAllAsync(CancellationToken ct = default) =>
        await context
            .Sessions.Include(s => s.SessionStudents)
                .ThenInclude(ss => ss.Student)
            .OrderByDescending(s => s.StartAt)
            .ToListAsync(ct);

    public async Task<bool> UpdateAsync(Session session, CancellationToken ct = default)
    {
        try
        {
            await context.SaveChangesAsync(ct);
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            return false;
        }
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var session = await context.Sessions.FindAsync(new object[] { id }, ct);
        if (session == null)
            return false;

        context.Sessions.Remove(session);
        await context.SaveChangesAsync(ct);
        return true;
    }
}
