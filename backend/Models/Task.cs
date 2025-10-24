    using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamApp.Models
{
    public class Task
    {
        [Key]
        public int TaskID { get; set; }

        // Foreign Keys + navigation properties

        [ForeignKey("Goal")]
        public int? GoalID_FK { get; set; }
        public virtual Project Goal { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 5)]
        public string Title { get; set; }
        [Required]
        [RegularExpression("^(Avslutad|Ej påbörjad|Påbörjad|Pausad)$", ErrorMessage = "Ogiltig status")]
        public string Status { get; set; }

        public string? Description { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public virtual ICollection<UserTask> UserTasks { get; set; }



    }
}
