using System.ComponentModel.DataAnnotations;
using TeamApp.Models;

namespace TeamApp.DTOs.TaskDTO
{
    public class CreateTaskDTO
    {

        [Required]
        public int? GoalID_FK { get; set; }
        [Required]
        public string Title { get; set; }

        public string? Description { get; set; }

        [Required]
        public DateTime DueDate { get; set; }
        
        public string? Status { get; set; }

        public int UserID_FK { get; set; }
    }
}
