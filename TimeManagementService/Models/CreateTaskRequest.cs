namespace TimeManagementService.Models;

public class CreateTaskRequest
{
    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? Tags { get; set; }

    public DateTime? DeadlineAt { get; set; }
}