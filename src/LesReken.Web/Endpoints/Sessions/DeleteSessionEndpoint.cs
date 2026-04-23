using FastEndpoints;
using LesReken.Application.Interfaces;

namespace LesReken.Web.Endpoints.Sessions;

public class DeleteSessionEndpoint(ISessionService sessionService) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Delete("/api/sessions/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var idString = Route<string>("id");
        if (!Guid.TryParse(idString, out var id))
        {
            await Send.ErrorsAsync(400, ct);
            return;
        }

        var result = await sessionService.DeleteSessionAsync(id, ct);
        if (result.IsSuccess)
        {
            await Send.NoContentAsync(ct);
        }
        else
        {
            await Send.NotFoundAsync(ct);
        }
    }
}
