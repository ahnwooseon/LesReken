using FastEndpoints;
using LesReken.Application.Interfaces;
using LesReken.Domain.Models;

namespace LesReken.Web.Endpoints.Sessions;

public class EndSessionEndpoint(ISessionService sessionService) : EndpointWithoutRequest<Session>
{
    public override void Configure()
    {
        Put("/api/sessions/{id}/end");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        Guid id = Route<Guid>("id");
        var result = await sessionService.EndSessionAsync(id, ct);

        if (result.IsSuccess)
        {
            await Send.OkAsync(result.Value, cancellation: ct);
        }
        else
        {
            AddError(result.Errors[0].Message);
            await Send.ErrorsAsync(400, ct);
        }
    }
}
