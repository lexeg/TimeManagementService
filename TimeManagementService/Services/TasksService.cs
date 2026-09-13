using Microsoft.EntityFrameworkCore;
using TimeManagementService.DataAccess.Contexts;
using TimeManagementService.DataAccess.Entities;

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

    public Task CreateTask(TaskEntity taskEntity)
    {
        _applicationDbContext.Tasks.Add(taskEntity);
        return _applicationDbContext.SaveChangesAsync();
    }

    public Task UpdateTask(TaskEntity taskEntity)
    {
        _applicationDbContext.Tasks.Update(taskEntity);
        return _applicationDbContext.SaveChangesAsync();
    }

    public Task DeleteTask(long taskId)
    {
        var entities = _applicationDbContext.Tasks.Where(x => x.Id == taskId).ToArray();
        _applicationDbContext.Tasks.RemoveRange(entities);
        return _applicationDbContext.SaveChangesAsync();
    }
}