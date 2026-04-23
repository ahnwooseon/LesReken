using System.Text.Json.Serialization;

namespace LesReken.Domain.Models;

public class Session
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public DateTimeOffset StartAt { get; set; }
    public DateTimeOffset? EndAt { get; set; }
    public decimal HourlyRate { get; set; }

    public List<SessionStudent> SessionStudents { get; set; } = new();
}

public class SessionStudent
{
    public Guid SessionId { get; set; }

    [JsonIgnore]
    public Session Session { get; set; } = null!;

    public Guid StudentId { get; set; }
    public Student Student { get; set; } = null!;
}
