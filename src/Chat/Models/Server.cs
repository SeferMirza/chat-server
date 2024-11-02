namespace Chat.Models;

public record Server(Guid ServerId, string ServerName)
{
    public int Capacity { get; set; } = 10;
    public List<User> ConnectedUsers { get; set; } = [];
}
