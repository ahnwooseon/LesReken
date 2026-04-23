namespace LesReken.Application.DTOs.Sessions;

public record StartSessionRequest(decimal HourlyRate, List<Guid> StudentIds);
