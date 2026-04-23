using FastEndpoints;
using FluentResults;
using LesReken.Application.Interfaces;
using LesReken.Domain.Models;

namespace LesReken.Web.Endpoints.Students;

public class UpdateStudentEndpoint(IStudentService studentService) : Endpoint<Student>
{
    public override void Configure()
    {
        Put("/api/students/{id:guid}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Student req, CancellationToken ct)
    {
        Guid id = Route<Guid>("id");
        if (id != req.Id)
        {
            await Send.ErrorsAsync(400, ct);
            return;
        }

        Result result = await studentService.UpdateAsync(req, ct);

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
