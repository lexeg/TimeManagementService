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

    public Task CreateTask(CreateTaskModel taskModel)
    {
        // TODO: add Mapster
        var taskEntity = new TaskEntity
        {
            Name = taskModel.Name,
            Description = taskModel.Description,
            Tags = taskModel.Tags,
            Status = StatusTypes.New,
            DeadlineAt = taskModel.DeadlineAt
        };
        _applicationDbContext.Tasks.Add(taskEntity);
        return _applicationDbContext.SaveChangesAsync();
    }

    public async Task UpdateTask(long taskId, TaskModel taskModel)
    {
        var taskEntity = await GetTaskById(taskId);
        if (taskEntity == null)
        {
            throw new KeyNotFoundException($"Task with id {taskId} not found");
        }

        taskEntity.Name = taskModel.Name;
        taskEntity.Description = taskModel.Description;
        taskEntity.Tags = taskModel.Tags;
        taskEntity.Status = taskModel.Status;
        taskEntity.DeadlineAt = taskModel.DeadlineAt;
        _applicationDbContext.Tasks.Update(taskEntity);
        await _applicationDbContext.SaveChangesAsync();
    }

    public Task DeleteTask(long taskId)
    {
        var entities = _applicationDbContext.Tasks
            .Where(x => x.Id == taskId)
            .ToArray();
        if (entities.Length == 0)
        {
            return Task.CompletedTask;
        }

        foreach (var entity in entities)
        {
            entity.DeletedAt = DateTime.UtcNow;
        }

        _applicationDbContext.Tasks.UpdateRange(entities);
        return _applicationDbContext.SaveChangesAsync();
    }
}