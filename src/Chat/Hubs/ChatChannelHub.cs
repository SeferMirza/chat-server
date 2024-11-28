using Chat.Exceptions;
using Chat.Models;
using Chat.Services;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Hubs;

public sealed class ChatChannelHub(IService _service) : Hub
{
    public async Task<List<Message>> JoinServer(string name, Guid serverId)
    {
        var userId = Context.ConnectionId;

        var server = _service.JoinServer(userId, name, serverId);
        await Groups.AddToGroupAsync(Context.ConnectionId, serverId.ToString());
        var currentDate = DateTime.UtcNow;
        server.Messages.RemoveAll(message => message.SentAt < currentDate.AddMonths(-1));

        var messageObj = new Message
        {
            Id = Guid.NewGuid(),
            Content = $"'{name}' has joined the server.",
            Sender = "Server",
            Server = server.ServerId,
            SentAt = DateTime.UtcNow
        };

        await Clients.Group(server.ServerId.ToString()).SendAsync("ReceiveMessage", messageObj);

        return await Task.FromResult(server.Messages);
    }

    public async Task LeaveServer(Guid serverId)
    {
        var server = _service.GetServer(serverId);
        var user = server.ConnectedUsers
            .Find(user => user.ConnectionId == Context.ConnectionId)  ?? throw new UserDidNotJoinThisServerException();

        _service.LeaveServer(user.ConnectionId, serverId);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, serverId.ToString());
    }

    public async Task SendMessage(Guid serverId, string message)
    {
        var server = _service.GetServer(serverId);
        var user = server.ConnectedUsers
            .Find(user => user.ConnectionId == Context.ConnectionId) ?? throw new UserDidNotJoinThisServerException();

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

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        (User user, ServerFullInfo server) = _service.Disconnect(Context.ConnectionId);

        var messageObj = new Message
        {
            Id = Guid.NewGuid(),
            Content = $"'{user.Name}' has left the server.",
            Sender = "Server",
            Server = server.ServerId,
            SentAt = DateTime.UtcNow
        };

        await Clients.Group(server.ServerId.ToString()).SendAsync("ReceiveMessage", messageObj);
        await base.OnDisconnectedAsync(exception);
    }
}
