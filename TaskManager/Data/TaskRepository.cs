using Microsoft.EntityFrameworkCore;
using TaskManagement.Models;

namespace TaskManagement.Data
{
    //BB: SaveChangesAsync should be revised per transaction, not per method
    //BB: Consider using a Unit of Work pattern for better transaction management
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskDbContext _context;

        public TaskRepository(TaskDbContext context)
        {
            _context = context;
        }

        public async Task<CustomTask> AddTaskAsync(CustomTask task)
        {
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<CustomTask> UpdateTaskAsync(CustomTask task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<CustomTask?> GetTaskByIdAsync(int id)
        {
            return await _context.Tasks.FindAsync(id);
        }

        public async Task<IEnumerable<CustomTask>> GetTasksAsync()
        {
            return await _context.Tasks.ToListAsync();
        }

        public async Task<CustomTask> DeleteTaskAsync(CustomTask task)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return task;
        }
    }
}
