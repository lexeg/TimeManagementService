using System.ComponentModel.DataAnnotations;
using TimeManagementService.DataAccess.Enums;

namespace TimeManagementService.Models;

public class UpdateTaskRequest
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = null!;

    [Required]
    public string Description { get; set; } = null!;

    public string? Tags { get; set; }

    public StatusTypes Status { get; set; }

    public DateTime? DeadlineAt { get; set; }
}