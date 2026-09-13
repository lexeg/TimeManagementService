using TimeManagementService.DataAccess.Enums;

namespace TimeManagementService.Models;

public class TaskModel
{
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? Tags { get; set; }

    public StatusTypes? Status { get; set; }

    public DateTime? DeadlineAt { get; set; }
}