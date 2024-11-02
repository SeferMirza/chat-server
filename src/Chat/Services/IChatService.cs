using Chat.Models;

namespace Chat.Services;

public interface IChatService
{
    Server GetServer(Guid serverId);
    Server CreateServer(string name);
    Server JoinServer(string userId, string userName, Guid serverId);
    bool LeaveServer(string userId, Guid serverId);
    void Disconnect(string userId);
}
