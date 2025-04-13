using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace TaskManagement.Models
{
    public class CustomTask
    {
        [SwaggerSchema("The unique identifier of the task.")]
        public int ID { get; private set; } // Make ID read-only

        [Required]
        [SwaggerSchema("The name of the task.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string? Name { get; set; }

        [SwaggerSchema("A detailed description of the task.")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        [Required]
        [SwaggerSchema("The current status of the task.")]
        public CustomTaskStatus Status { get; set; }
    }
}
