using LesReken.Application.Interfaces;
using LesReken.Domain.Models;
using LesReken.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LesReken.Infrastructure.Repositories;

public class PaymentRepository(ApplicationDbContext context) : IPaymentRepository
{
    public async Task<Payment> AddAsync(Payment payment, CancellationToken ct = default)
    {
        await context.Payments.AddAsync(payment, ct);
        await context.SaveChangesAsync(ct);
        return payment;
    }

    public async Task<IReadOnlyCollection<Payment>> GetAllAsync(CancellationToken ct = default)
    {
        return await context.Payments
            .Include(p => p.Student)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyCollection<Payment>> GetByStudentIdAsync(Guid studentId, CancellationToken ct = default)
    {
        return await context.Payments
            .Where(p => p.StudentId == studentId)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync(ct);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var payment = await context.Payments.FindAsync(new object[] { id }, ct);
        if (payment == null) return false;

        context.Payments.Remove(payment);
        var deleted = await context.SaveChangesAsync(ct);
        return deleted > 0;
    }

    public async Task<Payment?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await context.Payments
            .Include(p => p.Student)
            .FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task UpdateAsync(Payment payment, CancellationToken ct = default)
    {
        context.Payments.Update(payment);
        await context.SaveChangesAsync(ct);
    }
}
