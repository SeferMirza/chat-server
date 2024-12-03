using Chat.Exceptions;
using Chat.Models;

namespace Chat.Services;

public class ServerService : IService
{
    private readonly Dictionary<Guid, ServerFullInfo> _servers = [];

    public virtual ServerFullInfo CreateServer(string name, ServerType serverType, bool isPublic = true)
    {
        var server = new ServerFullInfo()
        {
            ServerId = Guid.NewGuid(),
            ServerName = name,
            ServerType = serverType,
            Public = isPublic
        };

        _servers[server.ServerId] = server;

        return server;
    }

    public virtual (User user, ServerFullInfo server) Disconnect(string connectionId)
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

    public virtual ServerFullInfo GetServer(Guid serverId)
    {
        if(!_servers.TryGetValue(serverId, out ServerFullInfo? server))
        {
            throw new ServerNotFoundException(serverId);
        }

        return server;
    }

    public virtual List<ServerDetailInfo> GetServers()
    {
        return _servers.Select(s =>
        {
            return new ServerDetailInfo()
            {
                ServerId = s.Key,
                ServerName = s.Value.ServerName,
                ServerType = s.Value.ServerType,
                Public = s.Value.Public,
                ConnectedUsers = s.Value.ConnectedUsers,
                Capacity = s.Value.Capacity
            };
        }).ToList();
    }

    public virtual ServerFullInfo JoinServer(string connectionId, string userName, Guid serverId)
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
