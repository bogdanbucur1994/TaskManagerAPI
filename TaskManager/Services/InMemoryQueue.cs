using System.Collections.Concurrent;

namespace TaskManagement.Services
{
    // BB: quick and dirty in-memory queue for mock-up purposes
    // BB: this should be replaced with a proper message queue in production code
    // BB: e.g., RabbitMQ, Azure Service Bus, etc.
    // BB: didn't want to lose too much time on this...
    public class InMemoryQueue : IMessageQueue
    {
        //BB: TODO replace object with proper structures
        private readonly ConcurrentQueue<object> _queue = new();
        private readonly ILogger<InMemoryQueue> _logger;

        public InMemoryQueue(ILogger<InMemoryQueue> logger)
        {
            _logger = logger;
        }

        public Task PublishAsync<T>(T message) where T : class
        {
            _queue.Enqueue(message);
            _logger.LogInformation("Message of type {MessageType} published to InMemoryQueue.", typeof(T).Name);
            return Task.CompletedTask;
        }

        public IEnumerable<object> GetAllMessages()
        {
            while (_queue.TryDequeue(out var message))
            {
                if (message != null)
                {
                    yield return message;
                }
            }
        }

        public IEnumerable<object> GetNextMessage()
        {
            _queue.TryDequeue(out var message);

            yield return message ?? "No messages in the queue.";
        }
    }
}
