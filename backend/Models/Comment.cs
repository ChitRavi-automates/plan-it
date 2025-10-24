using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamApp.Models
{
    public class Comment
    {
        [Key]
        public int CommentID { get; set; }

        [ForeignKey("User")]
        public int UserID_FK { get; set; }
        public virtual User User { get; set; }

        public int? TaskID { get; set; }
        public virtual Task? Task { get; set; }
        public int? TeamID { get; set; }
        public virtual Team? Team { get; set; }

        [StringLength(100, MinimumLength = 5)]
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
