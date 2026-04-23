using FastEndpoints;
using FluentResults;
using LesReken.Application.Interfaces;
using LesReken.Domain.Models;

namespace LesReken.Web.Endpoints.Students;

public class GetAllStudentsEndpoint(IStudentService studentService)
    : EndpointWithoutRequest<IReadOnlyCollection<Student>>
{
    public override void Configure()
    {
        Get("/api/students");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        Result<IReadOnlyCollection<Student>> result = await studentService.GetAllAsync(ct);

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
