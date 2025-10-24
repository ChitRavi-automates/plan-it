using System.ComponentModel.DataAnnotations;
using TeamApp.Models;

namespace TeamApp.DTOs.UserDTO
{
    public class UpdateUserDTO
    {
        [Required]
        public int UserID { get; set; }
        
        [StringLength(50, MinimumLength = 2, ErrorMessage ="Username måste vara mellan" +
            "2 och 50 tecken")]
        public string? Username { get; set; }
        
        [EmailAddress(ErrorMessage ="Ogiltig Email format!")]
        public string? Email { get; set; }

        [RegularExpression(@"^(User|Admin)$", ErrorMessage = "Ogiltig Roll (User, Admin)")]
        public string? Role { get; set; }

        // Optional password update fields
        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Lösenorden matchar inte!")]
        public string? ConfirmPassword { get; set; }

        //public virtual ICollection<UserTask> UserTasks { get; set; }
    }
}
