using System.ComponentModel.DataAnnotations;
using TeamApp.Models;

namespace TeamApp.DTOs.UserDTO
{
    public class CreateUserDTO
    {
        [Required]
        [EmailAddress(ErrorMessage = "Ogiltig Email format")]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage ="Måste vara mellan" +
            "2 och 50 tecken")]
        public string Username { get; set; }
        
        [Required(ErrorMessage ="Lösenord måste finnas!")]
        [StringLength(50, MinimumLength = 6, ErrorMessage ="Lösenorder behöver innehålla minst" +
            "6 tecken!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Lösenordet måste bekräftas!")]
        [Compare("Password", ErrorMessage = "Lösenorden matchar inte")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(500, ErrorMessage ="Beskrivningen är för lång, " +
            "max 500 tecken!")]
        public string Description { get; set; } // Ny property

        [RegularExpression(@"^(User|Admin)$", ErrorMessage = "Ogiltig Roll (User, Admin)")]
        public string Role { get; set; }


        //public virtual ICollection<UserTask> UserTasks { get; set; }
    }
}
