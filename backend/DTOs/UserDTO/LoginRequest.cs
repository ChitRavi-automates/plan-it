using System.ComponentModel.DataAnnotations;

namespace TeamApp.DTOs.UserDTO
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email behövs!")]
        [EmailAddress(ErrorMessage = "Ogiltigt email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Lösenord behövs!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
