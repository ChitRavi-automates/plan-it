using System.ComponentModel.DataAnnotations;

namespace TeamApp.DTOs.TeamDTO
{
    public class UpdateTeamDTO
    {
        [Required]
        public int UserID { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Description { get; set; }
        public string? Name { get; set; }
    }
}
