using FastEndpoints;
using FluentResults;
using LesReken.Application.Interfaces;
using LesReken.Domain.Models;

namespace LesReken.Web.Endpoints.Students;

public class GetStudentEndpoint(IStudentService studentService) : EndpointWithoutRequest<Student>
{
    public override void Configure()
    {
        Get("/api/students/{id:guid}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        Guid id = Route<Guid>("id");
        Result<Student> result = await studentService.GetByIdAsync(id, ct);

        if (result.IsSuccess)
        {
            await Send.OkAsync(result.Value, cancellation: ct);
        }
        else
        {
            await Send.NotFoundAsync(ct);
        }
    }
}
