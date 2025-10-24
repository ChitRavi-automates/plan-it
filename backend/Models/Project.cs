using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamApp.Models
{
    public class Project
    {
        [Key]
        public int ProjectID { get; set; }

        [ForeignKey("User")]
        public int UserID_FK { get; set; }
        public virtual User User { get; set; }

        [StringLength(100, MinimumLength = 8)]
        public string? Description { get; set; }

        [Required]
        public DateTime? Deadline { get; set; }
        public int? Status { get; set; }

        [StringLength(50, MinimumLength = 8)]
        public string Title { get; set; }

        public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();


    }
}
