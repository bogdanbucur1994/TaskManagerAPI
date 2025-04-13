using TaskManagement.Models;

namespace TaskManagement.Events
{
    public class TaskStatusUpdatedEvent
    {
        public int TaskId { get; set; }
        public CustomTaskStatus Status { get; set; }
    }
}
