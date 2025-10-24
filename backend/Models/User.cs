using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamApp.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        //Foreign Keys
        [ForeignKey("Team")]
        public int? TeamID_FK { get; set; }
        public virtual Team? Team { get; set; }

        [StringLength(50, MinimumLength = 2)]
        public string Username { get; set; }

        [StringLength(50, MinimumLength = 5)]
        public string PasswordHash { get; set; }//Lösenord bör inte lagras i text

        [EmailAddress]
        public string Email { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Role { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; } // Ny property

        // Relationer till andra entiteter
        //För att skapa 1 -> M relationer
        public virtual ICollection<Project> Goals { get; set; } = new List<Project>();
        public virtual ICollection<UserTask> UserTasks { get; set; }
        public virtual ICollection<LogEntry> LogEntries { get; set; } = new List<LogEntry>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
