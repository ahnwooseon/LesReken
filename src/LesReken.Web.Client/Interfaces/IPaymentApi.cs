using LesReken.Application.DTOs.Payments;
using LesReken.Domain.Models;
using Refit;

namespace LesReken.Web.Client.Interfaces;

public interface IPaymentApi
{
    [Post("/api/payments")]
    Task<Payment> CreateAsync([Body] CreatePaymentRequest req);

    [Get("/api/payments")]
    Task<List<Payment>> GetAllAsync();

    [Get("/api/payments/students/{studentId}/balance")]
    Task<decimal> GetBalanceAsync(Guid studentId);

    [Get("/api/payments/students/{studentId}/ledger")]
    Task<IEnumerable<StudentLedgerEntry>> GetLedgerAsync(Guid studentId);

    [Get("/api/payments/{id}")]
    Task<Payment> GetByIdAsync(Guid id);

    [Put("/api/payments/{id}")]
    Task UpdateAsync(Guid id, [Body] UpdatePaymentRequestDto req);

    [Delete("/api/payments/{id}")]
    Task DeleteAsync(Guid id);
}
