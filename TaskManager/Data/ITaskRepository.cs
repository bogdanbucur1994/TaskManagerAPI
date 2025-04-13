using TaskManagement.Models;

namespace TaskManagement.Data
{
    public interface ITaskRepository
    {
        Task<CustomTask> AddTaskAsync(CustomTask task);
        Task<CustomTask> UpdateTaskAsync(CustomTask task);
        Task<CustomTask?> GetTaskByIdAsync(int id);
        Task<IEnumerable<CustomTask>> GetTasksAsync();
        Task<CustomTask> DeleteTaskAsync(CustomTask task);
    }
}
