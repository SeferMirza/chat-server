using Chat.Models;

namespace Chat.Services;

public interface IService
{
    Server GetServer(Guid serverId);
    Server CreateServer(string name);
    Server JoinServer(string userId, string userName, Guid serverId);
    bool LeaveServer(string userId, Guid serverId);
    (User user, Server server) Disconnect(string userId);
    List<ServerInfo> GetServers();
    bool CheckUsername(Guid serverId, string username);
}