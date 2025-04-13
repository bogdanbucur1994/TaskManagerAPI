namespace TaskManagement.Services
{
    public interface IMessageBus
    {
        Task PublishAsync<T>(T message) where T : class;
    }

    public interface IMessageQueue
    {
        Task PublishAsync<T>(T message) where T : class;
    }

    public class MessageBus : IMessageBus
    {
        private readonly IMessageQueue _messageQueue;

        public MessageBus(IMessageQueue messageQueue)
        {
            _messageQueue = messageQueue;
        }

        public async Task PublishAsync<T>(T message) where T : class
        {
            await _messageQueue.PublishAsync(message);
        }
    }
}
