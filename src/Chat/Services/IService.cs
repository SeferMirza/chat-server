using Chat.Models;

namespace Chat.Services;

public interface IService
{
    ServerFullInfo GetServer(Guid serverId);
    ServerFullInfo CreateServer(string name, ServerType serverType, bool isPublic);
    ServerFullInfo JoinServer(string userId, string userName, Guid serverId);
    bool LeaveServer(string userId, Guid serverId);
    (User user, ServerFullInfo server) Disconnect(string userId);
    List<ServerCoreInfo> GetServers();
}
