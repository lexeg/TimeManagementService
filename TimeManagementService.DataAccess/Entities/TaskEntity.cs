using TimeManagementService.DataAccess.Enums;

namespace TimeManagementService.DataAccess.Entities;

public class TaskEntity
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? Tags { get; set; }

    public StatusTypes Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? DeadlineAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}