namespace LesReken.Application.DTOs.Payments;

public record UpdatePaymentRequestDto(
    Guid Id,
    Guid StudentId,
    decimal Amount,
    DateTimeOffset PaymentDate,
    string Method,
    string? Reference
);
