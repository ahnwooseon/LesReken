using FluentResults;
using LesReken.Application.DTOs.Payments;
using LesReken.Application.Interfaces;
using LesReken.Domain.Models;

namespace LesReken.Application.Services;

public class PaymentService(
    IPaymentRepository paymentRepository,
    ISessionRepository sessionRepository,
    IStudentRepository studentRepository) : IPaymentService
{
    public async Task<Result<Payment>> AddPaymentAsync(
        Guid studentId, 
        decimal amount, 
        DateTimeOffset paymentDate, 
        string method, 
        string? reference = null, 
        CancellationToken ct = default)
    {
        var student = await studentRepository.GetByIdAsync(studentId, ct);
        if (student == null) return Result.Fail("Leerling niet gevonden.");

        if (amount <= 0) return Result.Fail("Bedrag moet positief zijn.");

        var payment = new Payment
        {
            StudentId = studentId,
            Amount = amount,
            PaymentDate = paymentDate,
            Method = method,
            Reference = reference
        };

        var added = await paymentRepository.AddAsync(payment, ct);
        return Result.Ok(added);
    }

    public async Task<Result<IReadOnlyCollection<Payment>>> GetAllAsync(CancellationToken ct = default)
    {
        var payments = await paymentRepository.GetAllAsync(ct);
        return Result.Ok(payments);
    }

    public async Task<Result<IReadOnlyCollection<Payment>>> GetByStudentIdAsync(Guid studentId, CancellationToken ct = default)
    {
        var payments = await paymentRepository.GetByStudentIdAsync(studentId, ct);
        return Result.Ok(payments);
    }

    public async Task<Result> DeletePaymentAsync(Guid paymentId, CancellationToken ct = default)
    {
        var deleted = await paymentRepository.DeleteAsync(paymentId, ct);
        return deleted ? Result.Ok() : Result.Fail("Betaling niet gevonden.");
    }

    public async Task<Result<decimal>> GetStudentBalanceAsync(Guid studentId, CancellationToken ct = default)
    {
        var sessions = await sessionRepository.GetByStudentIdAsync(studentId, ct);
        
        decimal totalDebt = 0;
        foreach (var sess in sessions.Where(s => s.EndAt.HasValue))
        {
            var duration = GetDurationInHours(sess);
            var studentCount = sess.SessionStudents.Select(ss => ss.StudentId).Distinct().Count();
            if (studentCount > 0)
            {
                var sessionCost = sess.HourlyRate * (decimal)duration;
                var costPerStudent = Math.Round(sessionCost / studentCount, 2);
                totalDebt += costPerStudent;
            }
        }

        var payments = await paymentRepository.GetByStudentIdAsync(studentId, ct);
        decimal totalPaid = payments.Sum(p => p.Amount);

        return Result.Ok(totalDebt - totalPaid);
    }

    public async Task<Result<IReadOnlyCollection<StudentLedgerEntry>>> GetStudentLedgerAsync(Guid studentId, CancellationToken ct = default)
    {
        var ledger = new List<StudentLedgerEntry>();

        // 1. Sessions (Debits)
        var sessions = await sessionRepository.GetByStudentIdAsync(studentId, ct);
        foreach (var sess in sessions.Where(s => s.EndAt.HasValue))
        {
            var duration = GetDurationInHours(sess);
            var studentCount = sess.SessionStudents.Select(ss => ss.StudentId).Distinct().Count();
            if (studentCount > 0)
            {
                var sessionCost = sess.HourlyRate * (decimal)duration;
                var costPerStudent = Math.Round(sessionCost / studentCount, 2);
                
                var start = new DateTimeOffset(sess.StartAt.Year, sess.StartAt.Month, sess.StartAt.Day, sess.StartAt.Hour, sess.StartAt.Minute, 0, sess.StartAt.Offset); var end = new DateTimeOffset(sess.EndAt.Value.Year, sess.EndAt.Value.Month, sess.EndAt.Value.Day, sess.EndAt.Value.Hour, sess.EndAt.Value.Minute, 0, sess.EndAt.Value.Offset); var ts = end - start;
                var durationStr = ts.TotalMinutes < 60 ? $"{(int)ts.TotalMinutes}mn" : $"{(int)ts.TotalHours}u{ts.Minutes:D2}mn";

                ledger.Add(new StudentLedgerEntry(
                    sess.StartAt,
                    durationStr,
                    -costPerStudent,
                    false,
                    sess.Id
                ));
            }
        }

        // 2. Payments (Credits)
        var payments = await paymentRepository.GetByStudentIdAsync(studentId, ct);
        foreach (var p in payments)
        {
            ledger.Add(new StudentLedgerEntry(
                p.PaymentDate,
                p.Method,
                p.Amount,
                true,
                p.Id
            ));
        }

        return Result.Ok<IReadOnlyCollection<StudentLedgerEntry>>(ledger.OrderBy(l => l.Date).ToList());
    }

    private static double GetDurationInHours(Session s)
    {
        if (s.EndAt is null) return 0;
        // Truncate seconds to match UI logic
        var start = new DateTimeOffset(s.StartAt.Year, s.StartAt.Month, s.StartAt.Day, s.StartAt.Hour, s.StartAt.Minute, 0, s.StartAt.Offset);
        var end = new DateTimeOffset(s.EndAt.Value.Year, s.EndAt.Value.Month, s.EndAt.Value.Day, s.EndAt.Value.Hour, s.EndAt.Value.Minute, 0, s.EndAt.Value.Offset);
        return (end - start).TotalHours;
    }

    public async Task<Result<Payment>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var p = await paymentRepository.GetByIdAsync(id, ct);
        return p != null ? Result.Ok(p) : Result.Fail("Betaling niet gevonden.");
    }

    public async Task<Result> UpdatePaymentAsync(Guid id, Payment payment)
    {
        var existing = await paymentRepository.GetByIdAsync(id);
        if (existing == null) return Result.Fail("Betaling niet trouvé.");

        existing.StudentId = payment.StudentId;
        existing.Amount = payment.Amount;
        existing.PaymentDate = payment.PaymentDate;
        existing.Method = payment.Method;
        existing.Reference = payment.Reference;

        await paymentRepository.UpdateAsync(existing);
        return Result.Ok();
    }
}
