using TimeManagementService.DataAccess.Entities;
using TimeManagementService.Models;

namespace TimeManagementService.Services;

public interface ITasksService
{
    Task<TaskEntity[]> GetTasks();
    Task<TaskEntity?> GetTaskById(long taskId);
    Task CreateTask(CreateTaskRequest taskRequest);
    Task UpdateTask(long taskId, UpdateTaskRequest taskRequest);
    Task DeleteTask(long taskId);
}