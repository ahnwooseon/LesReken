using FastEndpoints;
using LesReken.Application.Interfaces;
using LesReken.Domain.Models;

namespace LesReken.Web.Endpoints.Sessions;

public class GetAllSessionsEndpoint(ISessionService sessionService)
    : EndpointWithoutRequest<IReadOnlyCollection<Session>>
{
    public override void Configure()
    {
        Get("/api/sessions");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await sessionService.GetAllAsync(ct);
        if (result.IsSuccess)
        {
            await Send.OkAsync(result.Value, cancellation: ct);
        }
        else
        {
            await Send.ErrorsAsync(500, ct);
        }
    }
}
