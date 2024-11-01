using Chat.DTOs;
using Chat.Models;

namespace Chat.Services;

public class ChatService : IChatService
{
    private readonly Dictionary<Guid, Server> _servers = [];

    public async Task<Server> CreateServerAsync(string creatorId, CreateServerDto dto)
    {
        var server = new Server(
            Guid.NewGuid(),
            creatorId,
            dto.Name
        );

        _servers[server.ServerId] = server;

        return server;
    }

    public async Task<Room> CreateRoomAsync(Guid serverId, CreateRoomDto dto)
    {
        if (!_servers.ContainsKey(serverId))
            throw new Exception("Server not found");

        var room = new Room(
            Guid.NewGuid(),
            dto.Name,
            [],
            dto.Type
        );

        _servers[serverId].Rooms.Add(room);

        return room;
    }

    public async Task<Room> JoinRoomAsync(string userId, JoinRoomDto dto)
    {
        if (!_servers.ContainsKey(dto.ServerId))
            throw new Exception("Server not found");

        var room = _servers[dto.ServerId].Rooms.FirstOrDefault(r => r.RoomId == dto.RoomId);
        if (room == null)
            throw new Exception("Room not found");

        if (!room.ConnectedUsers.Contains(userId))
            room.ConnectedUsers.Add(userId);

        return room;
    }

    public async Task<bool> LeaveRoomAsync(string userId, Guid roomId)
    {
        var server = _servers.Values.FirstOrDefault(s => s.Rooms.Any(r => r.RoomId == roomId));
        if (server == null)
            return false;

        var room = server.Rooms.FirstOrDefault(r => r.RoomId == roomId);
        if (room == null)
            return false;

        return room.ConnectedUsers.Remove(userId);
    }
}
