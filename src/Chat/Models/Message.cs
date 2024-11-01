namespace Chat.Models;

public class Message
{
    public Guid Id { get; set; }
    public string Content { get; set; } = "";
    public string SenderId { get; set; }
    public Guid RoomId { get; set; }
    public DateTime SentAt { get; set; }
}