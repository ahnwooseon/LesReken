namespace LesReken.Application.DTOs.Sessions;

public record UpdateSessionRequest(
    DateTimeOffset StartAt,
    DateTimeOffset? EndAt,
    decimal HourlyRate,
    List<Guid> StudentIds
);
