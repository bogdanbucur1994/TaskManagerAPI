using Microsoft.AspNetCore.Mvc;
using TaskManagement.Services;

namespace TaskManagement.Controllers
{
    [ApiController]
    [Route("api/messagebus")]
    public class MessageBusController : ControllerBase
    {
        private readonly InMemoryQueue _inMemoryQueue;

        public MessageBusController(InMemoryQueue inMemoryQueue)
        {
            _inMemoryQueue = inMemoryQueue;
        }

        /// <summary>
        /// Retrieves all messages from the in-memory queue.
        /// </summary>
        /// <remarks>
        /// This endpoint returns all messages currently stored in the in-memory queue.
        /// </remarks>
        [HttpGet("messages")]
        [ProducesResponseType(typeof(IEnumerable<object>), 200)] // Success response
        public IActionResult GetAllMessages()
        {
            var messages = _inMemoryQueue.GetAllMessages();
            return Ok(messages);
        }

        /// <summary>
        /// Retrieves the next message from the in-memory queue.
        /// </summary>
        /// <remarks>
        /// This endpoint returns the next message in the queue. If no messages are available, it returns a 404 status.
        /// </remarks>
        [HttpGet("messages/next")]
        [ProducesResponseType(typeof(object), 200)] // Success response
        [ProducesResponseType(404)] // Not found response
        public IActionResult GetNextMessage()
        {
            var message = _inMemoryQueue.GetNextMessage();
            if (message == null)
            {
                return NotFound("No messages available.");
            }
            return Ok(message);
        }
    }
}

