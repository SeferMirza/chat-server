using Chat.Models;

namespace Chat.Services;

public class ChatService : IChatService
{
    private readonly Dictionary<Guid, Server> _servers = [];

    public Server CreateServer(string name)
    {
        var server = new Server(
            Guid.NewGuid(),
            name
        );

        _servers[server.ServerId] = server;

        return server;
    }

    public void Disconnect(string connectionId)
    {
        var server = _servers.FirstOrDefault(s => s.Value.ConnectedUsers.Any(u => u.ConnectionId == connectionId)).Value;
        if (server != null)
        {
            var user = server.ConnectedUsers.First(u => u.ConnectionId == connectionId);
            server.ConnectedUsers.Remove(user);
        }
    }

    public Server GetServer(Guid serverId)
    {
        return _servers[serverId];
    }

    public Server JoinServer(string connectionId, string userName, Guid serverId)
    {
        if (!_servers.TryGetValue(serverId, out Server? server))
            throw new Exception("Server not found");

        if(!server.ConnectedUsers.Any(u => u.ConnectionId == connectionId))
        {
            if(server.ConnectedUsers.Any(u => u.Name == userName))
            {
                throw new Exception("This username already using in this server");
            }

            server.ConnectedUsers.Add(new(connectionId, userName));
        }

        return server;
    }

    public bool LeaveServer(string connectionId, Guid serverId)
    {
        var server = _servers[serverId] ?? throw new ServerNotFound();
        var user = server.ConnectedUsers
            .First(u => u.ConnectionId == connectionId) ?? throw new Exception("User not found");

        return server.ConnectedUsers.Remove(user);
    }
}
