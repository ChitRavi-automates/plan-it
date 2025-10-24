using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks.Dataflow;

namespace TeamApp.DTOs.NotificationDTO
{
    public class CreateNotiDTO
    {
        [Required]
        public int UserID { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Type { get; set; }
        
        public string? Message { get; set; }

        [Required]
        public DateTime Date { get; set; }

    }
}
