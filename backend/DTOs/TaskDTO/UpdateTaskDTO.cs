using System.ComponentModel.DataAnnotations;
using TeamApp.Models;

namespace TeamApp.DTOs.TaskDTO
{
    public class UpdateTaskDTO
    {
        public int? GoalID_FK { get; set; }
        public string? Title { get; set; }

        public string? Description { get; set; }
        [RegularExpression("^(Avslutad|Ej påbörjad|Påbörjad|Pausad)$", ErrorMessage = "Ogiltig status")]
        public string? Status { get; set; }
        public DateTime? DueDate { get; set; }
        public virtual ICollection<UserTask> UserTasks { get; set; }
    }
}