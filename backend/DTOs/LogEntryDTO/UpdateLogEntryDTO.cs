using System.ComponentModel.DataAnnotations;

namespace TeamApp.DTOs.LogEntryDTO
{
    public class UpdateLogEntryDTO
    {
        [Required]
        public int LogEntryID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        [StringLength(80)]
        public string Note { get; set; }

        [StringLength(50)]
        public string? Mood { get; set; }

    }
}
