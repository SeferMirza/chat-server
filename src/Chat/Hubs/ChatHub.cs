using Chat.DTOs;
using Chat.Models;
using Chat.Services;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Hubs;

public sealed class ChatHub(IChatService _chatService) : Hub
{
    public async Task<string> JoinRoom(Guid serverId, Guid roomId)
    {
        var userId = Context.ConnectionId;
        var joinRoomDto = new JoinRoomDto { ServerId = serverId, RoomId = roomId };

        await _chatService.JoinRoomAsync(userId, joinRoomDto);
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());

        return userId;
    }

    public async Task LeaveRoom(Guid roomId)
    {
        var userId = Context.ConnectionId;
        await _chatService.LeaveRoomAsync(userId, roomId);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId.ToString());
    }

    public async Task SendMessage(Guid roomId, string message)
    {
        var userId = Context.ConnectionId;

        var messageObj = new Message
        {
            Id = Guid.NewGuid(),
            Content = message,
            SenderId = userId,
            RoomId = roomId,
            SentAt = DateTime.UtcNow
        };

        await Clients.Group(roomId.ToString()).SendAsync("ReceiveMessage", messageObj);
    }
}