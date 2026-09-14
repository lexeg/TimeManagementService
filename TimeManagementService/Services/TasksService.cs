using Microsoft.EntityFrameworkCore;
using TimeManagementService.DataAccess.Contexts;
using TimeManagementService.DataAccess.Entities;
using TimeManagementService.DataAccess.Enums;
using TimeManagementService.Models;

namespace TimeManagementService.Services;

public class TasksService : ITasksService
{
    private readonly ApplicationDbContext _applicationDbContext;

    public TasksService(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public Task<TaskEntity[]> GetTasks()
    {
        return _applicationDbContext.Tasks
            .Where(x => x.DeletedAt == null)
            .ToArrayAsync();
    }

    public Task<TaskEntity?> GetTaskById(long taskId)
    {
        return _applicationDbContext.Tasks
            .Where(x => x.DeletedAt == null)
            .Where(x => x.Id == taskId)
            .FirstOrDefaultAsync();
    }

    public Task CreateTask(CreateTaskRequest taskRequest)
    {
        // TODO: add Mapster
        var taskEntity = new TaskEntity
        {
            Name = taskRequest.Title,
            Description = taskRequest.Description,
            Tags = taskRequest.Tags,
            Status = StatusTypes.New,
            DeadlineAt = taskRequest.DeadlineAt
        };
        _applicationDbContext.Tasks.Add(taskEntity);
        return _applicationDbContext.SaveChangesAsync();
    }

    public async Task UpdateTask(long taskId, UpdateTaskRequest taskRequest)
    {
        var taskEntity = await GetTaskById(taskId);
        if (taskEntity == null)
        {
            throw new KeyNotFoundException($"Task with id {taskId} not found");
        }

        taskEntity.Name = taskRequest.Title;
        taskEntity.Description = taskRequest.Description;
        taskEntity.Tags = taskRequest.Tags;
        taskEntity.Status = taskRequest.Status;
        taskEntity.DeadlineAt = taskRequest.DeadlineAt;
        _applicationDbContext.Tasks.Update(taskEntity);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task DeleteTask(long taskId)
    {
        var entity = await _applicationDbContext.Tasks
            .FirstOrDefaultAsync(x => x.Id == taskId);
        var entities = _applicationDbContext.Tasks
            .Where(x => x.Id == taskId)
            .ToArray();
        if (entity == null)
        {
            return;
        }

        entity.DeletedAt = DateTime.UtcNow;

        _applicationDbContext.Tasks.UpdateRange(entities);
        await _applicationDbContext.SaveChangesAsync();
    }
}