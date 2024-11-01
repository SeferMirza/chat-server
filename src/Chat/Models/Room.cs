namespace Chat.Models;

public record Room(Guid RoomId, string Name, List<string> ConnectedUsers, RoomType RoomType);

public enum RoomType
{
    Chat,
    Voice
}
