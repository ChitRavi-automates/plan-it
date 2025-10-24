using System.ComponentModel.DataAnnotations;
using TeamApp.Models;

namespace TeamApp.DTOs.TaskDTO
{
    public class TaskDTO
    {
        [Required]
        public int TaskID { get; set; }

        [Required]
        public int? GoalID_FK { get; set; }
        [Required]
        public string Title { get; set; }

        public string? Description { get; set; }
        
        public List<string>? TeamMembers { get; set; }
        
        //public string? TeamMember { get; set; }
        [Required]
        [RegularExpression("^(Avslutad|Ej påbörjad|Påbörjad|Pausad)$", ErrorMessage = "Ogiltig status")]

        public string Status { get; set; }
        [Required]
        public DateTime DueDate { get; set; }

        public virtual ICollection<UserTask> UserTasks { get; set; }
    }
}
