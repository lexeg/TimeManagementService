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
        return _applicationDbContext.Tasks.ToArrayAsync();
    }

    public Task<TaskEntity?> GetTaskById(long taskId)
    {
        return _applicationDbContext.Tasks.Where(x => x.Id == taskId).FirstOrDefaultAsync();
    }

    public Task CreateTask(CreateTaskModel taskModel)
    {
        // TODO: add Mapster
        var taskEntity = new TaskEntity
        {
            Name = taskModel.Name,
            Description = taskModel.Description,
            Tags = taskModel.Tags,
            Status = StatusTypes.InProgress,
            DeadlineAt = taskModel.DeadlineAt
        };
        _applicationDbContext.Tasks.Add(taskEntity);
        return _applicationDbContext.SaveChangesAsync();
    }

    public async Task UpdateTask(TaskModel taskModel)
    {
        var taskEntity = await GetTaskById(taskModel.Id);
        if (taskEntity == null)
        {
            throw new KeyNotFoundException($"Task with id {taskModel.Id} not found");
        }

        _applicationDbContext.Tasks.Update(taskEntity);
        await _applicationDbContext.SaveChangesAsync();
    }

    public Task DeleteTask(long taskId)
    {
        var entities = _applicationDbContext.Tasks.Where(x => x.Id == taskId).ToArray();
        _applicationDbContext.Tasks.RemoveRange(entities);
        return _applicationDbContext.SaveChangesAsync();
    }
}