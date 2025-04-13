using TaskManagement.Models;

namespace TaskManagement.Events
{
    public class TaskCreatedEvent
    {
        public int TaskId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public CustomTaskStatus Status { get; set; } = CustomTaskStatus.NotStarted;
    }
}
