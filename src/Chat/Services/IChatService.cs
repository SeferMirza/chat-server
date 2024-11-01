using Chat.DTOs;
using Chat.Models;

namespace Chat.Services;

public interface IChatService
{
    Task<Server> CreateServerAsync(string creatorId, CreateServerDto dto);
    Task<Room> CreateRoomAsync(Guid serverId, CreateRoomDto dto);
    Task<Room> JoinRoomAsync(string userId, JoinRoomDto dto);
    Task<bool> LeaveRoomAsync(string userId, Guid roomId);
}