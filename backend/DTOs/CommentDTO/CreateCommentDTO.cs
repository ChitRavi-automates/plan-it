namespace TeamApp.DTOs.CommentDTO
{
    public class CreateCommentDTO
    {
        public int UserID { get; set; }
        public int TaskID { get; set; }
        public string Text { get; set; }
    }
}
