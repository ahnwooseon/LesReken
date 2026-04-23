namespace LesReken.Domain.Models;

public class Student
{
    private string _name = string.Empty;

    public Guid Id { get; set; } = Guid.CreateVersion7();

    public string Name
    {
        get => _name;
        set => _name = value?.Trim() ?? string.Empty;
    }
}
