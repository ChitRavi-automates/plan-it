using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamApp.Models
{
    public class Notification
    {
        [Key]
        public int NotificationID { get; set; }

        [ForeignKey("User")]
        public int UserID_FK { get; set; }
        public virtual User User { get; set; }

        public string Type { get; set; }
        public DateTime CreatedAt { get; set; }
        
        [StringLength(100, MinimumLength = 5)]
        public string? Message { get; set; }
        public bool IsRead { get; set; }
    }
}
