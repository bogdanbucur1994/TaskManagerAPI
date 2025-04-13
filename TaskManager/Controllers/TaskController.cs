using Microsoft.AspNetCore.Mvc;
using TaskManagement.Services;
using TaskManagement.Models;
using System.ComponentModel;

namespace TaskManagement.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<TaskController> _logger;

        public TaskController(ITaskService taskService, ILogger<TaskController> logger)
        {
            _taskService = taskService;
            _logger = logger;
            _logger.LogInformation("TaskController initialized.");
        }

        /// <summary>
        /// Adds a new task.
        /// </summary>
        /// <remarks>
        /// This endpoint creates a new task and publishes a message to the message bus.
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(CustomTask), 201)] // Created response
        [ProducesResponseType(400)] // Bad request response
        public async Task<IActionResult> CreateTask([FromBody] CustomTask task)
        {
            _logger.LogInformation("Creating a new task.");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for task creation.");
                return BadRequest();
            }

            var createdTask = await _taskService.AddTaskAsync(task);
            _logger.LogInformation("Task created with ID {TaskId}.", createdTask.ID);
            return CreatedAtAction(nameof(GetTaskById), new { id = createdTask.ID }, createdTask);
        }

        /// <summary>
        /// Retrieves a task by its ID.
        /// </summary>
        /// <remarks>
        /// This endpoint fetches the details of a specific task using its unique ID.
        /// </remarks>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CustomTask), 200)] // Success response
        [ProducesResponseType(404)] // Not found response
        public async Task<IActionResult> GetTaskById(int id)
        {
            _logger.LogInformation("Fetching task with ID {TaskId}.", id);
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
            {
                _logger.LogWarning("Task with ID {TaskId} not found.", id);
                return NotFound();
            }

            return Ok(task);
        }

        /// <summary>
        /// Retrieves all tasks.
        /// </summary>
        /// <remarks>
        /// This endpoint returns a list of all tasks in the system.
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CustomTask>), 200)] // Success response
        public async Task<IActionResult> GetAllTasks()
        {
            _logger.LogInformation("Fetching all tasks.");
            var tasks = await _taskService.GetTasksAsync();
            return Ok(tasks);
        }

        /// <summary>
        /// Updates the status of an existing task.
        /// </summary>
        /// <remarks>
        /// This endpoint updates the status of a task and publishes a message to the message bus.
        /// </remarks>
        [HttpPatch("{id}/status")]
        [ProducesResponseType(typeof(CustomTask), 200)] // Success response
        [ProducesResponseType(404)] // Not found response
        public async Task<IActionResult> UpdateTaskStatus(int id, [FromBody] CustomTaskStatus status)
        {
            _logger.LogInformation("Updating status of task with ID {TaskId} to {Status}.", id, status);
            var updatedTask = await _taskService.UpdateTaskAsync(id, status);
            if (updatedTask == null)
            {
                _logger.LogWarning("Task with ID {TaskId} not found for status update.", id);
                return NotFound();
            }

            return Ok(updatedTask);
        }

        /// <summary>
        /// Deletes a task by its ID.
        /// </summary>
        /// <remarks>
        /// This endpoint deletes a specific task using its unique ID.
        /// </remarks>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)] // No content response
        [ProducesResponseType(404)] // Not found response
        [Description("Deletes a task by its ID.")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            _logger.LogInformation("Deleting task with ID {TaskId}.", id);
            var isDeleted = await _taskService.DeleteTaskAsync(id);
            if (!isDeleted)
            {
                _logger.LogWarning("Task with ID {TaskId} not found for deletion.", id);
                return NotFound();
            }

            return NoContent();
        }
    }
}