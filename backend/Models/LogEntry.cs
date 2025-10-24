using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamApp.Models
{
    public class LogEntry
    {
        [Key]
        public int LogEntryID { get; set; }
        
        [ForeignKey("User")]
        public int UserID_FK { get; set; }
        public virtual User User { get; set; }

        public DateTime Date { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string? Mood { get; set; }

        [StringLength(100, MinimumLength = 5)]
        public string? Note { get; set; }
    }
}
