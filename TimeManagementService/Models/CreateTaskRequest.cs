using System.ComponentModel.DataAnnotations;

namespace TimeManagementService.Models;

public class CreateTaskRequest
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = null!;

    [Required]
    public string Description { get; set; } = null!;

    public string? Tags { get; set; }

    public DateTime? DeadlineAt { get; set; }
}