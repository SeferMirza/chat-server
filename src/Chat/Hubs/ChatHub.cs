using Chat.Models;
using Chat.Services;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Hubs;

public sealed class ChatHub(IChatService _chatService) : Hub
{
    public async Task<List<Message>> JoinServer(string name, Guid serverId)
    {
        var userId = Context.ConnectionId;

        var server = _chatService.JoinServer(userId, name, serverId);
        await Groups.AddToGroupAsync(Context.ConnectionId, serverId.ToString());
        var currentDate = DateTime.UtcNow;
        server.Messages.RemoveAll(message => message.SentAt < currentDate.AddMonths(-1));

        return await Task.FromResult(server.Messages);
    }

    public async Task LeaveServer(Guid serverId)
    {
        var userId = Context.ConnectionId;
        _chatService.LeaveServer(userId, serverId);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, serverId.ToString());
    }

    public async Task SendMessage(Guid serverId, string message)
    {
        var server = _chatService.GetServer(serverId);
        var user = server.ConnectedUsers
            .Find(user => user.ConnectionId == Context.ConnectionId) ?? throw new Exception("User not joined this server");

        var messageObj = new Message
        {
            Id = Guid.NewGuid(),
            Content = message,
            Sender = user.Name,
            Server = server.ServerId,
            SentAt = DateTime.UtcNow
        };

        await Clients.Group(server.ServerId.ToString()).SendAsync("ReceiveMessage", messageObj);

        server.Messages.Add(messageObj);
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _chatService.Disconnect(Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }
}
