using TimeManagementService.DataAccess.Enums;

namespace TimeManagementService.Models;

public class TaskResponse
{
    public long Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? Tags { get; set; }

    public StatusTypes Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? DeadlineAt { get; set; }
}