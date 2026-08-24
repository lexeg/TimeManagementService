using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeManagementService.DataAccess;

namespace TimeManagementService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly ILogger<TasksController> _logger;

    public TasksController(ApplicationDbContext applicationDbContext, ILogger<TasksController> logger)
    {
        _applicationDbContext = applicationDbContext;
        _logger = logger;
    }

    [HttpPost("tasks")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<HttpResponseMessage> CreateTask([FromBody] TaskEntity task)
    {
        if (task == null)
            return new HttpResponseMessage(HttpStatusCode.BadRequest);

        _applicationDbContext.Tasks.Add(task);
        await _applicationDbContext.SaveChangesAsync();
        return new HttpResponseMessage(HttpStatusCode.NoContent);
    }

    [HttpDelete("tasks/{taskId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<HttpResponseMessage> DeleteTask(long taskId)
    {
        var entities = _applicationDbContext.Tasks.Where(x => x.Id == taskId).ToArray();
        _applicationDbContext.Tasks.RemoveRange(entities);
        await _applicationDbContext.SaveChangesAsync();
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
            _applicationDbContext.Tasks.Update(task);
            await _applicationDbContext.SaveChangesAsync();
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
        var tasks = await _applicationDbContext.Tasks.ToArrayAsync();
        return tasks;
    }

    [HttpGet("tasks/{id:long}")]
    [ProducesResponseType(typeof(TaskEntity), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTaskById(long id)
    {
        var task = await _applicationDbContext.Tasks.Where(x => x.Id == id).FirstOrDefaultAsync();
        if (task == null)
        {
            return NotFound();
        }

        return Ok(task);
    }
}