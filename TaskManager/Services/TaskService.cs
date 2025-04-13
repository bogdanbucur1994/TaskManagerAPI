using TaskManagement.Data;
using TaskManagement.Events;
using TaskManagement.Models;

namespace TaskManagement.Services
{
    public interface ITaskService
    {
        Task<CustomTask> AddTaskAsync(CustomTask task);
        Task<CustomTask> UpdateTaskAsync(int id, CustomTaskStatus status);
        Task<CustomTask> GetTaskByIdAsync(int id);
        Task<IEnumerable<CustomTask>> GetTasksAsync();
        Task<bool> DeleteTaskAsync(int id);
    }

    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repository;
        private readonly IMessageBus _messageBus;
        private readonly ILogger<TaskService> _logger;

        public TaskService(ITaskRepository repository, IMessageBus messageBus, ILogger<TaskService> logger)
        {
            _repository = repository;
            _messageBus = messageBus;
            _logger = logger;
            _logger.LogInformation("TaskService initialized.");
        }

        public async Task<CustomTask> AddTaskAsync(CustomTask task)
        {
            _logger.LogInformation("Adding a new task.");
            // The database will handle ID generation
            var addedTask = await _repository.AddTaskAsync(task);

            _logger.LogInformation("Task added with ID {TaskId}.", addedTask.ID);

            // Publish a message using the custom MessageBus
            await _messageBus.PublishAsync(new TaskCreatedEvent
            {
                TaskId = addedTask.ID,
                Name = addedTask.Name ?? string.Empty,
                Description = addedTask.Description ?? string.Empty,
                Status = addedTask.Status
            });

            _logger.LogInformation("TaskCreatedEvent published for Task ID {TaskId}.", addedTask.ID);

            return addedTask;
        }

        public async Task<CustomTask> UpdateTaskAsync(int id, CustomTaskStatus status)
        {
            _logger.LogInformation("Updating task with ID {TaskId} to status {Status}.", id, status);
            var task = await _repository.GetTaskByIdAsync(id);
            if (task == null)
            {
                throw new KeyNotFoundException($"Task with ID {id} was not found."); // Throw exception if task is not found
            }

            task.Status = status;
            var updatedTask = await _repository.UpdateTaskAsync(task);

            _logger.LogInformation("Task with ID {TaskId} updated to status {Status}.", id, status);

            // Publish a message using the custom MessageBus
            await _messageBus.PublishAsync(new TaskStatusUpdatedEvent
            {
                TaskId = id,
                Status = status
            });

            _logger.LogInformation("TaskStatusUpdatedEvent published for Task ID {TaskId}.", id);

            return updatedTask;
        }

        public async Task<IEnumerable<CustomTask>> GetTasksAsync()
        {
            return await _repository.GetTasksAsync();
        }

        public async Task<CustomTask> GetTaskByIdAsync(int id)
        {
            var task = await _repository.GetTaskByIdAsync(id);
            if (task == null)
            {
                throw new KeyNotFoundException($"Task with ID {id} was not found.");
            }
            return task;
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await _repository.GetTaskByIdAsync(id);
            if (task == null)
            {
                return false; // Return false if task is not found
            }

            await _repository.DeleteTaskAsync(task);
            return true;
        }
    }
}
