using TaskManagement.Models;

namespace TaskManagement.Data
{
    public static class DatabaseSeeder
    {
        public static void Seed(TaskDbContext dbContext)
        {
            if (!dbContext.Tasks.Any())
            {
                dbContext.Tasks.AddRange(new[]
                {
                    new CustomTask
                    {
                        Name = "Task 1",
                        Description = "Description for Task 1",
                        Status = CustomTaskStatus.NotStarted
                    },
                    new CustomTask
                    {
                        Name = "Task 2",
                        Description = "Description for Task 2",
                        Status = CustomTaskStatus.InProgress
                    }
                });
                dbContext.SaveChanges();
            }
        }
    }
}
