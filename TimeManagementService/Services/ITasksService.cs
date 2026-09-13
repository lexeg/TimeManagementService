using TimeManagementService.DataAccess.Entities;
using TimeManagementService.Models;

namespace TimeManagementService.Services;

public interface ITasksService
{
    Task<TaskEntity[]> GetTasks();
    Task<TaskEntity?> GetTaskById(long taskId);
    Task CreateTask(CreateTaskModel taskModel);
    Task UpdateTask(long taskId, TaskModel taskModel);
    Task DeleteTask(long taskId);
}