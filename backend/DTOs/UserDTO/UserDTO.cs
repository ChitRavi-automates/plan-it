using TeamApp.Models;

namespace TeamApp.DTOs.UserDTO
{
    public class UserDTO
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string? Role { get; set; }
        public string? Description { get; set; } // Ny property
        public virtual ICollection<UserTask> UserTasks { get; set; }
    }
}
