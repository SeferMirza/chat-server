namespace Chat.Models;

public class Message
{
    public Guid Id { get; set; }
    public string Content { get; set; } = "";
    public string? Sender { get; set; }
    public Guid? Server { get; set; }
    public DateTime SentAt { get; set; }
}
