using LesReken.Application.DTOs.Sessions;
using LesReken.Domain.Models;
using Refit;

namespace LesReken.Web.Client.Interfaces;

public interface ISessionApi
{
    [Post("/api/sessions")]
    Task<Session> StartAsync([Body] StartSessionRequest req);

    [Put("/api/sessions/{id}/end")]
    Task<Session> EndAsync(Guid id);

    [Put("/api/sessions/{id}")]
    Task<Session> UpdateAsync(Guid id, [Body] UpdateSessionRequest req);

    [Get("/api/sessions")]
    Task<List<Session>> GetAllAsync();

    [Delete("/api/sessions/{id}")]
    Task DeleteAsync(Guid id);
}
