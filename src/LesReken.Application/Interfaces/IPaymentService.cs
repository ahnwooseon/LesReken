using FluentResults;
using LesReken.Application.DTOs.Payments;
using LesReken.Domain.Models;

namespace LesReken.Application.Interfaces;

public interface IPaymentService
{
    Task<Result<Payment>> AddPaymentAsync(Guid studentId, decimal amount, DateTimeOffset paymentDate, string method, string? reference = null, CancellationToken ct = default);
    Task<Result<IReadOnlyCollection<Payment>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<IReadOnlyCollection<StudentLedgerEntry>>> GetStudentLedgerAsync(Guid studentId, CancellationToken ct = default);
    Task<Result<Payment>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Result> UpdatePaymentAsync(Guid id, Payment payment);
    Task<Result> DeletePaymentAsync(Guid paymentId, CancellationToken ct = default);
    Task<Result<decimal>> GetStudentBalanceAsync(Guid studentId, CancellationToken ct = default);
}
