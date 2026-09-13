using TimeManagementService.DataAccess.Entities;

namespace TimeManagementService.Services;

public interface ITasksService
{
    Task<TaskEntity[]> GetTasks();
    Task<TaskEntity?> GetTaskById(long taskId);
    Task CreateTask(TaskEntity taskEntity);
    Task UpdateTask(TaskEntity taskEntity);
    Task DeleteTask(long taskId);
}