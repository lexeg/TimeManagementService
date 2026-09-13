namespace TimeManagementService.Models;

public class CreateTaskModel
{
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? Tags { get; set; }

    public DateTime? DeadlineAt { get; set; }
}