namespace LesReken.Application.DTOs.Payments;

public record CreatePaymentRequest(
    Guid StudentId,
    decimal Amount,
    DateTimeOffset PaymentDate,
    string Method,
    string? Reference
);
