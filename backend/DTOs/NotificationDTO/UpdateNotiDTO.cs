using System.ComponentModel.DataAnnotations;

namespace TeamApp.DTOs.NotificationDTO
{
    public class UpdateNotiDTO
    {
        [Required]
        public int UserID { get; set; }
        
        [Required]
        public int NotificationID { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; }

        public string? Message { get; set; }

        [Required]
        public DateTime Date { get; set; }

    }
}
