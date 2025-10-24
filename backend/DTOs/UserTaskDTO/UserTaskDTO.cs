using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TeamApp.Models;

namespace TeamApp.DTOs.UserTaskDTO
{
    public class UserTaskDTO
    {
        [Required]
        [ForeignKey("User")]
        public int userID_FK { get; set; }
        public virtual User user { get; set; }
        [Required]
        [ForeignKey("Task")]
        public int taskID_FK { get; set; }
        public virtual Models.Task task { get; set; }
    }
}
