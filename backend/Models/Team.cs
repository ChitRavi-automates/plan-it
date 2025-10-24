using System.ComponentModel.DataAnnotations;

namespace TeamApp.Models
{
    public class Team
    {
        [Key]
        public int TeamID { get; set; }

        [StringLength(100, MinimumLength = 5)]
        public string Description { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 2)]
        public string Name { get; set; }

        // Navigation property – ett team har många användare
        public virtual ICollection<User> Users { get; set; } = new List<User>();

    }
}
