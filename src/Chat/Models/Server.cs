namespace Chat.Models;

public record Server(Guid ServerId, string OwnerId, string ServerName)
{
    public int Capacity { get; set; } = 10;
    public List<Room> Rooms { get; set; } = [];
}
