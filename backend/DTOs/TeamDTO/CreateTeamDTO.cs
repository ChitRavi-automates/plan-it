using System.ComponentModel.DataAnnotations;

namespace TeamApp.DTOs.TeamDTO
{
    public class CreateTeamDTO
    {
        [Required]
        public int TeamID { get; set; }
        [Required]
        public int UserID { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Description { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Name { get; set; }
    }
}
