using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Logging;
using TaskManagement.Services;
using System.Linq;
using System;

namespace Intaker.Tests
{
    // BB:  NOT WORKING, TODO
    [TestFixture]
    public class InMemoryQueueTests
    {
        private InMemoryQueue _queue;
        private Mock<ILogger<InMemoryQueue>> _loggerMock;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<InMemoryQueue>>();
            _queue = new InMemoryQueue(_loggerMock.Object);
        }

        [Test]
        public void PublishAsync_ShouldAddMessageToQueue()
        {
            // ...existing code...
        }

        [Test]
        public void GetAllMessages_ShouldReturnAllMessagesInQueue()
        {
            // ...existing code...
        }

        [Test]
        public void GetAllMessages_ShouldReturnEmpty_WhenQueueIsEmpty()
        {
            // Act
            var messages = _queue.GetAllMessages();

            // Assert
            Assert.IsEmpty(messages);
        }

        [Test]
        public void PublishAsync_ShouldLogMessage()
        {
            // Arrange
            var message = new { Content = "Test Message" };

            // Act
            _queue.PublishAsync(message).Wait();

            // Assert
            _loggerMock.Verify(logger => logger.LogInformation(
                It.Is<string>(s => s.Contains("Message of type")),
                It.IsAny<object[]>()), Times.Once);
        }

        [Test]
        public void GetAllMessages_ShouldDequeueMessagesInOrder()
        {
            // Arrange
            var message1 = new { Content = "Message 1" };
            var message2 = new { Content = "Message 2" };
            _queue.PublishAsync(message1).Wait();
            _queue.PublishAsync(message2).Wait();

            // Act
            var messages = _queue.GetAllMessages().ToList();

            // Assert
            Assert.AreEqual(message1, messages[0]);
            Assert.AreEqual(message2, messages[1]);
        }

        [Test]
        public void PublishAsync_ShouldThrowException_WhenMessageIsNull()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _queue.PublishAsync<object>(null));
        }

        [Test]
        public void GetAllMessages_ShouldReturnEmptyList_WhenQueueIsEmpty()
        {
            // Act
            var messages = _queue.GetAllMessages();

            // Assert
            Assert.IsEmpty(messages);
        }

        [Test]
        public void PublishAsync_ShouldLogMessage_WhenMessageIsPublished()
        {
            // Arrange
            var message = new { Content = "Test Message" };

            // Act
            _queue.PublishAsync(message).Wait();

            // Assert
            _loggerMock.Verify(logger => logger.LogInformation(
                It.Is<string>(s => s.Contains("Message of type")),
                It.IsAny<object[]>()), Times.Once);
        }
    }
}