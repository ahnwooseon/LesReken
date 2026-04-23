namespace LesReken.Application.DTOs.Payments;

public record StudentLedgerEntry(
    DateTimeOffset Date,
    string Description,
    decimal Amount,
    bool IsPayment,
    Guid RelatedId
);
