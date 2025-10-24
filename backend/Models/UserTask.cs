using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamApp.Models
{
    public class UserTask
    {
        [Key]
        public int userTaskID { get; set; }

        [Required]
        [ForeignKey("User")]
        public int userID_FK { get; set; }
        public virtual User user { get; set; }
        [Required]
        [ForeignKey("Task")]
        public int taskID_FK { get; set; }
        public virtual Task task { get; set; }

    }
}