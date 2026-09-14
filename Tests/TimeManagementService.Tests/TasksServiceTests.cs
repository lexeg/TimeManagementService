using Microsoft.EntityFrameworkCore;
using TimeManagementService.DataAccess.Contexts;
using TimeManagementService.DataAccess.Entities;
using TimeManagementService.DataAccess.Enums;
using TimeManagementService.Models;
using TimeManagementService.Services;
using Xunit;

namespace TimeManagementService.Tests;

public class TasksServiceTests
{
    [Fact]
    public async Task CreateTask_CreatesTask()
    {
        await using var dbContext = CreateDbContext();
        var service = new TasksService(dbContext);

        var model = new CreateTaskRequest
        {
            Title = "Test task",
            Description = "Test description",
            Tags = "test",
            DeadlineAt = DateTime.UtcNow.AddDays(1)
        };

        await service.CreateTask(model);

        var task = await dbContext.Tasks.SingleAsync();

        Assert.Equal("Test task", task.Name);
        Assert.Equal("Test description", task.Description);
        Assert.Equal("test", task.Tags);
        Assert.Equal(StatusTypes.New, task.Status);
    }

    [Fact]
    public async Task GetTasks_ReturnsOnlyNotDeletedTasks()
    {
        await using var dbContext = CreateDbContext();

        dbContext.Tasks.AddRange(
            new TaskEntity
            {
                Name = "Active task",
                Description = "Active",
                Status = StatusTypes.New
            },
            new TaskEntity
            {
                Name = "Deleted task",
                Description = "Deleted",
                Status = StatusTypes.New,
                DeletedAt = DateTime.UtcNow
            });

        await dbContext.SaveChangesAsync();

        var service = new TasksService(dbContext);

        var tasks = await service.GetTasks();

        var task = Assert.Single(tasks);

        Assert.Equal("Active task", task.Name);
    }

    [Fact]
    public async Task GetTaskById_WhenTaskExists_ReturnsTask()
    {
        await using var dbContext = CreateDbContext();

        var task = new TaskEntity
        {
            Name = "Test task",
            Description = "Test description",
            Status = StatusTypes.New
        };

        dbContext.Tasks.Add(task);
        await dbContext.SaveChangesAsync();

        var service = new TasksService(dbContext);

        var result = await service.GetTaskById(task.Id);

        Assert.NotNull(result);
        Assert.Equal(task.Id, result.Id);
        Assert.Equal("Test task", result.Name);
    }

    [Fact]
    public async Task GetTaskById_WhenTaskDoesNotExist_ReturnsNull()
    {
        await using var dbContext = CreateDbContext();
        var service = new TasksService(dbContext);

        var result = await service.GetTaskById(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateTask_UpdatesTaskFields()
    {
        await using var dbContext = CreateDbContext();

        var task = new TaskEntity
        {
            Name = "Old name",
            Description = "Old description",
            Tags = "old",
            Status = StatusTypes.New,
            DeadlineAt = DateTime.UtcNow.AddDays(1)
        };

        dbContext.Tasks.Add(task);
        await dbContext.SaveChangesAsync();

        var service = new TasksService(dbContext);

        var model = new UpdateTaskRequest
        {
            Title = "New name",
            Description = "New description",
            Tags = "new",
            Status = StatusTypes.Completed,
            DeadlineAt = DateTime.UtcNow.AddDays(2)
        };

        await service.UpdateTask(task.Id, model);

        var updatedTask = await dbContext.Tasks
            .SingleAsync(x => x.Id == task.Id);

        Assert.Equal("New name", updatedTask.Name);
        Assert.Equal("New description", updatedTask.Description);
        Assert.Equal("new", updatedTask.Tags);
        Assert.Equal(StatusTypes.Completed, updatedTask.Status);
        Assert.Equal(model.DeadlineAt, updatedTask.DeadlineAt);
    }

    [Fact]
    public async Task UpdateTask_WhenTaskDoesNotExist_ThrowsKeyNotFoundException()
    {
        await using var dbContext = CreateDbContext();
        var service = new TasksService(dbContext);

        var model = new UpdateTaskRequest
        {
            Title = "Test task",
            Description = "Test description",
            Status = StatusTypes.InProgress
        };

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => service.UpdateTask(999, model));
    }

    [Fact]
    public async Task DeleteTask_SetsDeletedAt()
    {
        await using var dbContext = CreateDbContext();

        var task = new TaskEntity
        {
            Name = "Test task",
            Description = "Test description",
            Status = StatusTypes.New
        };

        dbContext.Tasks.Add(task);
        await dbContext.SaveChangesAsync();

        var service = new TasksService(dbContext);

        await service.DeleteTask(task.Id);

        var deletedTask = await dbContext.Tasks
            .SingleAsync();
        var result = await service.GetTaskById(task.Id);

        Assert.NotNull(deletedTask.DeletedAt);
        Assert.Null(result);
    }

    private static ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }
}