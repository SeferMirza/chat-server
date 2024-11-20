using Chat.Exceptions;
using Chat.Services;

namespace Chat.Test;

public class ChatServiceTests
{
    ChatService _chatService;

    [SetUp]
    public void SetUp()
    {
        _chatService = new ChatService();
    }

    [Test]
    public void CreateServer_Should_Add_Server_To_Dictionary_When_Called()
    {
        var serverName = "Test Server";

        var server = _chatService.CreateServer(serverName);

        var result = _chatService.GetServer(server.ServerId);
        Assert.That(serverName, Is.EqualTo(result.ServerName));
    }

    [Test]
    public void JoinServer_Should_Add_User_To_Server_When_Valid_Request()
    {
        var server = _chatService.CreateServer("Test Server");
        var connectionId = "connection-1";
        var userName = "TestUser";

        var result = _chatService.JoinServer(connectionId, userName, server.ServerId);

        Assert.That(result.ConnectedUsers.Any(u => u.ConnectionId == connectionId && u.Name == userName), Is.True);
    }

    [Test]
    public void LeaveServer_Should_Remove_User_From_Server_When_Valid_Request()
    {
        var server = _chatService.CreateServer("Test Server");
        var connectionId = "connection-1";
        var userName = "TestUser";
        _chatService.JoinServer(connectionId, userName, server.ServerId);

        var result = _chatService.LeaveServer(connectionId, server.ServerId);

        Assert.That(result, Is.True);
        Assert.That(server.ConnectedUsers.Any(u => u.ConnectionId == connectionId), Is.False);
    }

    [Test]
    public void CheckUsername_Should_Return_True_When_Username_Not_Taken()
    {
        var server = _chatService.CreateServer("Test Server");

        var result = _chatService.CheckUsername(server.ServerId, "NewUser");

        Assert.That(result, Is.True);
    }

    [Test]
    public void CheckUsername_Should_Throw_Exception_When_Server_Not_Found()
    {
        var invalidServerId = Guid.NewGuid();

        Assert.That(
            () => _chatService.CheckUsername(invalidServerId, "User"),
            Throws.Exception.TypeOf<ServerNotFoundException>()
        );
    }
}