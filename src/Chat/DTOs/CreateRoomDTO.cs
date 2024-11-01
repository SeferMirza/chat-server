using Chat.Models;

namespace Chat.DTOs;

public class CreateRoomDto
{
    public string Name { get; set; } = "My Room";
    public Guid ServerId { get; set; }
    public RoomType Type { get; set; }
}
