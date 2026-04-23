using FluentResults;
using LesReken.Application.DTOs.Sessions;
using LesReken.Application.Interfaces;
using LesReken.Domain.Models;
using LesReken.Web.Client.Interfaces;
using Refit;

namespace LesReken.Web.Client.Services;

public class WasmSessionService(ISessionApi api) : ISessionService
{
    public async Task<Result<Session>> StartSessionAsync(
        decimal hourlyRate,
        List<Guid> studentIds,
        CancellationToken ct = default
    )
    {
        try
        {
            var session = await api.StartAsync(new StartSessionRequest(hourlyRate, studentIds));
            return Result.Ok(session);
        }
        catch (ApiException ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public async Task<Result<Session>> EndSessionAsync(
        Guid sessionId,
        CancellationToken ct = default
    )
    {
        try
        {
            var session = await api.EndAsync(sessionId);
            return Result.Ok(session);
        }
        catch (ApiException ex)
        {
            return Result.Fail(ex.Message);
        }
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
        try
        {
            var session = await api.UpdateAsync(
                sessionId,
                new UpdateSessionRequest(startAt, endAt, hourlyRate, studentIds)
            );
            return Result.Ok(session);
        }
        catch (ApiException ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public async Task<Result<IReadOnlyCollection<Session>>> GetAllAsync(
        CancellationToken ct = default
    )
    {
        try
        {
            var sessions = await api.GetAllAsync();
            return Result.Ok<IReadOnlyCollection<Session>>(sessions);
        }
        catch (ApiException ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public async Task<Result<Session>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var sessions = await api.GetAllAsync();
        var session = sessions.FirstOrDefault(s => s.Id == id);
        return session == null ? Result.Fail("Session introuvable.") : Result.Ok(session);
    }

    public async Task<Result> DeleteSessionAsync(Guid sessionId, CancellationToken ct = default)
    {
        try
        {
            await api.DeleteAsync(sessionId);
            return Result.Ok();
        }
        catch (ApiException ex)
        {
            return Result.Fail(ex.Message);
        }
    }
}
