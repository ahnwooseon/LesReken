using FluentResults;
using LesReken.Application.Interfaces;
using LesReken.Domain.Models;
using LesReken.Web.Client.Interfaces;
using Refit;

namespace LesReken.Web.Client.Services;

public class WasmStudentService(IStudentApi api) : IStudentService
{
    public async Task<Result<Student>> AddAsync(Student student, CancellationToken ct = default)
    {
        try
        {
            Student created = await api.CreateAsync(student);
            return Result.Ok(created);
        }
        catch (ApiException ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public async Task<Result<Student>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        try
        {
            Student student = await api.GetByIdAsync(id);
            return Result.Ok(student);
        }
        catch (ApiException ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public async Task<Result<IReadOnlyCollection<Student>>> GetAllAsync(
        CancellationToken ct = default
    )
    {
        try
        {
            List<Student> students = await api.GetAllAsync();
            return Result.Ok<IReadOnlyCollection<Student>>(students);
        }
        catch (ApiException ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public async Task<Result> UpdateAsync(Student student, CancellationToken ct = default)
    {
        try
        {
            await api.UpdateAsync(student.Id, student);
            return Result.Ok();
        }
        catch (ApiException ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        try
        {
            await api.DeleteAsync(id);
            return Result.Ok();
        }
        catch (ApiException ex)
        {
            return Result.Fail(ex.Message);
        }
    }
}
