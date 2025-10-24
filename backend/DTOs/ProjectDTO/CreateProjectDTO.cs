using System.ComponentModel.DataAnnotations;

namespace TeamApp.DTOs.ProjectDTO
{
    public class CreateProjectDTO
    {
        [Required]
        public int UserID { get; set; }
        [Required]
        [StringLength(80)]
        public string Description { get; set; }

        [Required]
        public string Title { get; set; }
    }
}
