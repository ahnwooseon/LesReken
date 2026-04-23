using FastEndpoints;
using FluentResults;
using LesReken.Application.Interfaces;
using LesReken.Domain.Models;

namespace LesReken.Web.Endpoints.Students;

public class CreateStudentEndpoint(IStudentService studentService) : Endpoint<Student, Student>
{
    public override void Configure()
    {
        Post("/api/students");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Student req, CancellationToken ct)
    {
        Result<Student> result = await studentService.AddAsync(req, ct);

        if (result.IsSuccess)
        {
            await Send.CreatedAtAsync<GetStudentEndpoint>(
                new { id = result.Value.Id },
                result.Value,
                cancellation: ct
            );
        }
        else
        {
            AddError(result.Errors[0].Message);
            await Send.ErrorsAsync(400, ct);
        }
    }
}
