namespace LesReken.Domain.Models;

public class Payment
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = null!;
    
    public decimal Amount { get; set; }
    public DateTimeOffset PaymentDate { get; set; }
    public string Method { get; set; } = "Virement"; // Cash, Payconiq, Virement, etc.
    public string? Reference { get; set; }
}
