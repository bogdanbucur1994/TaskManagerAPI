using NUnit.Framework;
using Moq;
using TaskManagement.Models;
using TaskManagement.Services;
using TaskManagement.Data;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Intaker.Tests
{
    // BB: NOT WORKING, TODO
    [TestFixture]
    public class TaskServiceTests
    {
        private Mock<ITaskRepository> _taskRepositoryMock;
        private Mock<IMessageBus> _messageBusMock;
        private Mock<ILogger<TaskService>> _loggerMock;
        private TaskService _taskService;

        [SetUp]
        public void SetUp()
        {
            _taskRepositoryMock = new Mock<ITaskRepository>();
            _messageBusMock = new Mock<IMessageBus>();
            _loggerMock = new Mock<ILogger<TaskService>>();
            _taskService = new TaskService(_taskRepositoryMock.Object, _messageBusMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task AddTaskAsync_ShouldAddTaskAndPublishMessage()
        {
            // Arrange
            var task = new CustomTask { Name = "New Task" };
            _taskRepositoryMock.Setup(repo => repo.AddTaskAsync(It.IsAny<CustomTask>())).Returns(Task.FromResult(task));

            // Act
            await _taskService.AddTaskAsync(task);

            // Assert
            _taskRepositoryMock.Verify(repo => repo.AddTaskAsync(task), Times.Once);
            _messageBusMock.Verify(bus => bus.PublishAsync(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task UpdateTaskAsync_ShouldUpdateTaskAndPublishMessage()
        {
            // Arrange
            var task = new CustomTask { Name = "Updated Task" };
            _taskRepositoryMock.Setup(repo => repo.UpdateTaskAsync(It.IsAny<CustomTask>())).Returns(Task.FromResult(task));

            // Act
            await _taskService.UpdateTaskAsync(0, CustomTaskStatus.Completed);

            // Assert
            _taskRepositoryMock.Verify(repo => repo.UpdateTaskAsync(task), Times.Once);
            _messageBusMock.Verify(bus => bus.PublishAsync(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task GetTaskByIdAsync_ShouldReturnCorrectTask()
        {
            // Arrange
            var task = new CustomTask { Name = "Test Task" };
            _taskRepositoryMock.Setup(repo => repo.GetTaskByIdAsync(task.ID)).ReturnsAsync(task);

            // Act
            var result = await _taskService.GetTaskByIdAsync(task.ID);

            // Assert
            Assert.AreEqual(task, result);
            _taskRepositoryMock.Verify(repo => repo.GetTaskByIdAsync(task.ID), Times.Once);
        }

        [Test]
        public async Task GetTasksAsync_ShouldReturnAllTasks()
        {
            // Arrange
            var tasks = new List<CustomTask>
            {
                new CustomTask { Name = "Task 1" },
                new CustomTask { Name = "Task 2" }
            };
            _taskRepositoryMock.Setup(repo => repo.GetTasksAsync()).ReturnsAsync(tasks);

            // Act
            var result = await _taskService.GetTasksAsync();

            // Assert
            Assert.AreEqual(tasks, result);
            _taskRepositoryMock.Verify(repo => repo.GetTasksAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteTaskAsync_ShouldDeleteTask_WhenTaskExists()
        {
            // Arrange
            var task = new CustomTask { Name = "Task to Delete" };
            _taskRepositoryMock.Setup(repo => repo.GetTaskByIdAsync(task.ID)).ReturnsAsync(task);
            _taskRepositoryMock.Setup(repo => repo.DeleteTaskAsync(task)).Returns(Task.FromResult(task));

            // Act
            var result = await _taskService.DeleteTaskAsync(task.ID);

            // Assert
            Assert.IsTrue(result);
            _taskRepositoryMock.Verify(repo => repo.DeleteTaskAsync(task), Times.Once);
        }

        [Test]
        public async Task DeleteTaskAsync_ShouldReturnFalse_WhenTaskDoesNotExist()
        {
            // Arrange
            _taskRepositoryMock.Setup(repo => repo.GetTaskByIdAsync(It.IsAny<int>())).ReturnsAsync((CustomTask)null);

            // Act
            var result = await _taskService.DeleteTaskAsync(1);

            // Assert
            Assert.IsFalse(result);
            _taskRepositoryMock.Verify(repo => repo.DeleteTaskAsync(It.IsAny<CustomTask>()), Times.Never);
        }

        [Test]
        public void AddTaskAsync_ShouldThrowException_WhenTaskIsNull()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _taskService.AddTaskAsync(null));
        }

        [Test]
        public void GetTaskByIdAsync_ShouldThrowException_WhenTaskDoesNotExist()
        {
            // Arrange
            _taskRepositoryMock.Setup(repo => repo.GetTaskByIdAsync(It.IsAny<int>())).ReturnsAsync((CustomTask)null);

            // Act & Assert
            Assert.ThrowsAsync<KeyNotFoundException>(async () => await _taskService.GetTaskByIdAsync(1));
        }
    }
}