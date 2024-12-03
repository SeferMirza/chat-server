using Chat.Exceptions;
using Chat.Models;
using Chat.Services;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Hubs;

public sealed class VoiceChannelHub(IService _service) : Hub
{
    public async Task Connect(string name, Guid serverId)
    {
        _service.JoinServer(Context.ConnectionId, name, serverId);

        await Groups.AddToGroupAsync(Context.ConnectionId, serverId.ToString());

        await Clients.Group(serverId.ToString()).SendAsync("UserJoined", new User(Context.ConnectionId, name));
    }

    public async Task LeaveServer(Guid serverId)
    {
        var server = _service.GetServer(serverId);
        var user = server.ConnectedUsers
            .Find(user => user.ConnectionId == Context.ConnectionId)  ?? throw new UserDidNotJoinThisServerException();

        _service.LeaveServer(user.ConnectionId, serverId);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, serverId.ToString());
    }

    public async Task SendVoiceData(Guid serverId, byte[] audioData)
    {
        var server = _service.GetServer(serverId);

        await Clients.OthersInGroup(server.ServerId.ToString()).SendAsync("ReceiveVoiceData", Context.ConnectionId, audioData);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        (User user, ServerFullInfo server) = _service.Disconnect(Context.ConnectionId);

        await Clients.OthersInGroup(server.ServerId.ToString()).SendAsync("UserDisconnected", user);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, server.ServerId.ToString());

        await base.OnDisconnectedAsync(exception);
    }
}
