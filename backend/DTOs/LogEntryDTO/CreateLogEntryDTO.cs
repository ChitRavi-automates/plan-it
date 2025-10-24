using System.ComponentModel.DataAnnotations;

namespace TeamApp.DTOs.LogEntryDTO
{
    public class CreateLogEntryDTO
    {
        [Required]
        public int UserID { get; set; }

        [Required]
        public string Note { get; set; }

        public string? Mood { get; set; }
        public DateTime Date { get; set; }
    }
}
