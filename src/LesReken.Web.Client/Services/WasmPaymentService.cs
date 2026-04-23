using FluentResults;
using LesReken.Application.DTOs.Payments;
using LesReken.Application.Interfaces;
using LesReken.Domain.Models;
using LesReken.Web.Client.Interfaces;
using Refit;

namespace LesReken.Web.Client.Services;

public class WasmPaymentService(IPaymentApi api) : IPaymentService
{
    public async Task<Result<Payment>> AddPaymentAsync(
        Guid studentId, 
        decimal amount, 
        DateTimeOffset paymentDate, 
        string method, 
        string? reference = null, 
        CancellationToken ct = default)
    {
        try
        {
            var payment = await api.CreateAsync(new CreatePaymentRequest(studentId, amount, paymentDate, method, reference));
            return Result.Ok(payment);
        }
        catch (ApiException ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public async Task<Result<IReadOnlyCollection<Payment>>> GetAllAsync(CancellationToken ct = default)
    {
        try
        {
            var payments = await api.GetAllAsync();
            return Result.Ok<IReadOnlyCollection<Payment>>(payments);
        }
        catch (ApiException ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public async Task<Result<IReadOnlyCollection<Payment>>> GetByStudentIdAsync(Guid studentId, CancellationToken ct = default)
    {
        // For simplicity, we filter the list of all payments or we could add a dedicated endpoint.
        // Let's filter for now to avoid multiplying endpoints.
        var result = await GetAllAsync(ct);
        if (result.IsFailed) return result;
        return Result.Ok<IReadOnlyCollection<Payment>>(result.Value.Where(p => p.StudentId == studentId).ToList());
    }

    public async Task<Result> DeletePaymentAsync(Guid paymentId, CancellationToken ct = default)
    {
        try
        {
            await api.DeleteAsync(paymentId);
            return Result.Ok();
        }
        catch (ApiException ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public async Task<Result<decimal>> GetStudentBalanceAsync(Guid studentId, CancellationToken ct = default)
    {
        try
        {
            var balance = await api.GetBalanceAsync(studentId);
            return Result.Ok(balance);
        }
        catch (ApiException ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public async Task<Result<IReadOnlyCollection<StudentLedgerEntry>>> GetStudentLedgerAsync(Guid studentId, CancellationToken ct = default)
    {
        try
        {
            var entries = await api.GetLedgerAsync(studentId);
            return Result.Ok<IReadOnlyCollection<StudentLedgerEntry>>(entries.ToList().AsReadOnly());
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public async Task<Result<Payment>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        try
        {
            var p = await api.GetByIdAsync(id);
            return Result.Ok(p);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public async Task<Result> UpdatePaymentAsync(Guid id, Payment payment)
    {
        try
        {
            var req = new UpdatePaymentRequestDto(
                id,
                payment.StudentId,
                payment.Amount,
                payment.PaymentDate,
                payment.Method,
                payment.Reference
            );
            await api.UpdateAsync(id, req);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }
}
