using Chat.Exceptions;
using Chat.Models;

namespace Chat.Services;

public abstract class ChannelService : IService
{
    private readonly Dictionary<Guid, Server> _servers = [];

    public virtual bool CheckUsername(Guid serverId, string username)
    {
        if(!_servers.TryGetValue(serverId, out Server? server))
        {
            throw new ServerNotFoundException(serverId);
        }

        return !server.ConnectedUsers.Any(x => x.Name == username);
    }

    public virtual Server CreateServer(string name)
    {
        var server = new Server(
            Guid.NewGuid(),
            name
        );

        _servers[server.ServerId] = server;

        return server;
    }

    public virtual (User user, Server server) Disconnect(string connectionId)
    {
        var server = _servers.FirstOrDefault(s => s.Value.ConnectedUsers.Any(u => u.ConnectionId == connectionId)).Value;
        if (server == null)
        {
            throw new ServerNotFoundException(server!.ServerId);
        }

        var user = server.ConnectedUsers.First(u => u.ConnectionId == connectionId);
        server.ConnectedUsers.Remove(user);

        return (user, server);
    }

    public virtual Server GetServer(Guid serverId)
    {
        if(!_servers.TryGetValue(serverId, out Server? server))
        {
            throw new ServerNotFoundException(serverId);
        }

        return server;
    }

    public virtual List<ServerInfo> GetServers()
    {
        return _servers.Select(s => new ServerInfo(s.Key, s.Value.ServerName)).ToList();
    }

    public virtual Server JoinServer(string connectionId, string userName, Guid serverId)
    {
        var server = GetServer(serverId);

        if(!server.ConnectedUsers.Any(u => u.ConnectionId == connectionId))
        {
            if(server.ConnectedUsers.Any(u => u.Name == userName))
            {
                throw new UsernameAlreadyInUseException(userName);
            }

            server.ConnectedUsers.Add(new(connectionId, userName));
        }

        return server;
    }

    public virtual bool LeaveServer(string connectionId, Guid serverId)
    {
        var server = GetServer(serverId);
        var user = server.ConnectedUsers
            .First(u => u.ConnectionId == connectionId) ?? throw new UserNotFoundException();

        return server.ConnectedUsers.Remove(user);
    }
}