using System.ComponentModel.DataAnnotations;

namespace TeamApp.DTOs.ProjectDTO
{
    public class UpdateProjectDTO
    {
        [Required]
        public int UserID { get; set; }

        [Required]
        public int ProjectID { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]
        [StringLength(80)]
        public string Description { get; set; }

    }
}
