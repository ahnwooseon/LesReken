using FastEndpoints;
using LesReken.Application.DTOs.Sessions;
using LesReken.Application.Interfaces;
using LesReken.Domain.Models;

namespace LesReken.Web.Endpoints.Sessions;

public class UpdateSessionEndpoint(ISessionService sessionService)
    : Endpoint<UpdateSessionRequest, Session>
{
    public override void Configure()
    {
        Put("/api/sessions/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateSessionRequest req, CancellationToken ct)
    {
        Guid id = Route<Guid>("id");
        var result = await sessionService.UpdateSessionAsync(
            id,
            req.StartAt,
            req.EndAt,
            req.HourlyRate,
            req.StudentIds,
            ct
        );

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
