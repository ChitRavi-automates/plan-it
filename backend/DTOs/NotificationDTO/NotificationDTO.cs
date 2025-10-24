namespace TeamApp.DTOs.NotificationDTO
{
    public class NotificationDTO
    {
        public int NotificationID { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
    }
}
