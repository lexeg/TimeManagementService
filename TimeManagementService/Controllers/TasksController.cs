using System.Net;
using Microsoft.AspNetCore.Mvc;
using TimeManagementService.DataAccess.Entities;
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
    public async Task<HttpResponseMessage> CreateTask([FromBody] TaskEntity task)
    {
        if (task == null)
            return new HttpResponseMessage(HttpStatusCode.BadRequest);

        await _tasksService.CreateTask(task);
        return new HttpResponseMessage(HttpStatusCode.NoContent);
    }

    [HttpDelete("tasks/{taskId:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<HttpResponseMessage> DeleteTask(long taskId)
    {
        await _tasksService.DeleteTask(taskId);
        return new HttpResponseMessage(HttpStatusCode.NoContent);
    }

    [HttpPut("tasks")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<HttpResponseMessage> UpdateTask([FromBody] TaskEntity task)
    {
        if (task == null)
            return new HttpResponseMessage(HttpStatusCode.BadRequest);

        try
        {
            await _tasksService.UpdateTask(task);
        }
        catch (ArgumentNullException)
        {
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        return new HttpResponseMessage(HttpStatusCode.NoContent);
    }

    [HttpGet("tasks")]
    [ProducesResponseType(typeof(TaskEntity[]), StatusCodes.Status200OK)]
    public async Task<TaskEntity[]> GetTasks()
    {
        _logger.Log(LogLevel.Information, "get all tasks");
        var tasks = await _tasksService.GetTasks();
        return tasks;
    }

    [HttpGet("tasks/{id:long}")]
    [ProducesResponseType(typeof(TaskEntity), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTaskById(long id)
    {
        var task = await _tasksService.GetTaskById(id);
        if (task == null)
        {
            return NotFound();
        }

        return Ok(task);
    }
}