using FastEndpoints;
using FluentResults;
using LesReken.Application.Interfaces;

namespace LesReken.Web.Endpoints.Students;

public class DeleteStudentEndpoint(IStudentService studentService) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Delete("/api/students/{id:guid}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        Guid id = Route<Guid>("id");
        Result result = await studentService.DeleteAsync(id, ct);

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
