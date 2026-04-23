using LesReken.Domain.Models;

namespace LesReken.Application.Interfaces;

public interface IPaymentRepository
{
    Task<Payment> AddAsync(Payment payment, CancellationToken ct = default);
    Task<IReadOnlyCollection<Payment>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyCollection<Payment>> GetByStudentIdAsync(Guid studentId, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
    Task<Payment?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task UpdateAsync(Payment payment, CancellationToken ct = default);
}
