using FastEndpoints;
using LesReken.Application.DTOs.Sessions;
using LesReken.Application.Interfaces;
using LesReken.Domain.Models;

namespace LesReken.Web.Endpoints.Sessions;

public class StartSessionEndpoint(ISessionService sessionService)
    : Endpoint<StartSessionRequest, Session>
{
    public override void Configure()
    {
        Post("/api/sessions");
        AllowAnonymous();
    }

    public override async Task HandleAsync(StartSessionRequest req, CancellationToken ct)
    {
        var result = await sessionService.StartSessionAsync(req.HourlyRate, req.StudentIds, ct);

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
