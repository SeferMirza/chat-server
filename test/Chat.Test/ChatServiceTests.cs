using Chat.Exceptions;
using Chat.Services;

namespace Chat.Test;

public class ChatServiceTests
{
    ServerService _service;

    [SetUp]
    public void SetUp()
    {
        _service = new ServerService();
    }

    [Test]
    public void CreateServer_Should_Add_Server_To_Dictionary_When_Called()
    {
        var serverName = "Test Server";

        var server = _service.CreateServer(serverName, Models.ServerType.Chat);

        var result = _service.GetServer(server.ServerId);
        Assert.That(serverName, Is.EqualTo(result.ServerName));
    }

    [Test]
    public void JoinServer_Should_Add_User_To_Server_When_Valid_Request()
    {
        var server = _service.CreateServer("Test Server", Models.ServerType.Chat);
        var connectionId = "connection-1";
        var userName = "TestUser";

        var result = _service.JoinServer(connectionId, userName, server.ServerId);

        Assert.That(result.ConnectedUsers.Any(u => u.ConnectionId == connectionId && u.Name == userName), Is.True);
    }

    [Test]
    public void LeaveServer_Should_Remove_User_From_Server_When_Valid_Request()
    {
        var server = _service.CreateServer("Test Server", Models.ServerType.Chat);
        var connectionId = "connection-1";
        var userName = "TestUser";
        _service.JoinServer(connectionId, userName, server.ServerId);

        var result = _service.LeaveServer(connectionId, server.ServerId);

        Assert.That(result, Is.True);
        Assert.That(server.ConnectedUsers.Any(u => u.ConnectionId == connectionId), Is.False);
    }
}