using LesReken.Domain.Models;
using Refit;

namespace LesReken.Web.Client.Interfaces;

public interface IStudentApi
{
    [Post("/api/students")]
    Task<Student> CreateAsync([Body] Student student);

    [Get("/api/students/{id}")]
    Task<Student> GetByIdAsync(Guid id);

    [Get("/api/students")]
    Task<List<Student>> GetAllAsync();

    [Put("/api/students/{id}")]
    Task UpdateAsync(Guid id, [Body] Student student);

    [Delete("/api/students/{id}")]
    Task DeleteAsync(Guid id);
}
