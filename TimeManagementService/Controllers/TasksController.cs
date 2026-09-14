using System.Net;
using Microsoft.AspNetCore.Mvc;
using TimeManagementService.DataAccess.Entities;
using TimeManagementService.Models;
using TimeManagementService.Services;

namespace TimeManagementService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITasksService _tasksService;
    private readonly ILogger<TasksController> _logger;

    public TasksController(ITasksService tasksService, ILogger<TasksController> logger)
    {
        _tasksService = tasksService;
        _logger = logger;
    }

    [HttpPost("tasks")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest taskRequest)
    {
        if (taskRequest == null)
            return BadRequest();

        await _tasksService.CreateTask(taskRequest);
        return NoContent();
    }

    [HttpDelete("tasks/{taskId:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<HttpResponseMessage> DeleteTask(long taskId)
    {
        await _tasksService.DeleteTask(taskId);
        return new HttpResponseMessage(HttpStatusCode.NoContent);
    }

    [HttpPut("tasks/{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<HttpResponseMessage> UpdateTask([FromRoute] long id, [FromBody] UpdateTaskRequest taskRequest)
    {
        if (taskRequest == null)
            return new HttpResponseMessage(HttpStatusCode.BadRequest);

        try
        {
            await _tasksService.UpdateTask(id, taskRequest);
        }
        catch (KeyNotFoundException exception)
        {
            _logger.LogWarning(exception, "Task with id {TaskModelId} not found", id);
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }
        catch (ArgumentNullException)
        {
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        return new HttpResponseMessage(HttpStatusCode.NoContent);
    }

    [HttpGet("tasks")]
    [ProducesResponseType(typeof(TaskResponse[]), StatusCodes.Status200OK)]
    public async Task<TaskResponse[]> GetTasks()
    {
        _logger.Log(LogLevel.Information, "get all tasks");
        var tasks = await _tasksService.GetTasks();
        return [.. tasks.Select(ToTaskResponse)];
    }

    [HttpGet("tasks/{id:long}")]
    [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTaskById(long id)
    {
        var task = await _tasksService.GetTaskById(id);
        if (task == null)
        {
            return NotFound();
        }

        return Ok(ToTaskResponse(task));
    }

    private static TaskResponse ToTaskResponse(TaskEntity entity) =>
        new()
        {
            Id = entity.Id,
            Title = entity.Name,
            Description = entity.Description,
            Tags = entity.Tags,
            Status = entity.Status,
            CreatedAt = entity.CreatedAt,
            DeadlineAt = entity.DeadlineAt
        };
}