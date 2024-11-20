using Chat.Hubs;
using Chat.Models;
using Chat.Services;
using Microsoft.AspNetCore.SignalR;
using Moq;

namespace Chat.Test;

public class ChatHubTests
{
    private Mock<IChatService> _chatServiceMock;
    private ChatHub _chatHub;
    private Mock<IHubCallerClients> _clientsMock;
    private Mock<IGroupManager> _groupsMock;

    [SetUp]
    public void SetUp()
    {
        _chatServiceMock = new Mock<IChatService>();
        _clientsMock = new Mock<IHubCallerClients>();
        _groupsMock = new Mock<IGroupManager>();

        _chatHub = new ChatHub(_chatServiceMock.Object)
        {
            Clients = _clientsMock.Object,
            Groups = _groupsMock.Object,
            Context = Mock.Of<HubCallerContext>(c => c.ConnectionId == "connection-1")
        };
    }

    [TearDown]
    public void TearDown()
    {
        _chatHub.Dispose();
    }


    [Test]
    public async Task JoinServer_Should_Add_User_To_Server_And_Return_Messages()
    {
        var serverId = Guid.NewGuid();
        var server = new Server(serverId, "Test Server");
        server.Messages.Add(new Message { Content = "Welcome!", SentAt = DateTime.UtcNow });

        _chatServiceMock
            .Setup(s => s.JoinServer("connection-1", "TestUser", serverId))
            .Returns(server);

        var result = await _chatHub.JoinServer("TestUser", serverId);

        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result[0].Content, Is.EqualTo("Welcome!"));
        _groupsMock.Verify(g => g.AddToGroupAsync("connection-1", serverId.ToString(), default), Times.Once);
    }

    [Test]
    public async Task SendMessage_Should_Broadcast_Message_To_Group()
    {
        var serverId = Guid.NewGuid();
        var server = new Server(serverId, "Test Server");
        server.ConnectedUsers.Add(new("connection-1", "TestUser"));

        _chatServiceMock.Setup(s => s.GetServer(serverId)).Returns(server);

        var clientsGroupMock = new Mock<IClientProxy>();
        _clientsMock.Setup(c => c.Group(serverId.ToString())).Returns(clientsGroupMock.Object);

        await _chatHub.SendMessage(serverId, "Hello!");

        clientsGroupMock.Verify(
            c => c.SendCoreAsync("ReceiveMessage",
                It.Is<object[]>(o => o.Length == 1 && ((Message)o[0]).Content == "Hello!"),
                default),
            Times.Once);

        Assert.That(server.Messages.Count, Is.EqualTo(1));
        Assert.That(server.Messages[0].Content, Is.EqualTo("Hello!"));
    }

    [Test]
    public async Task OnDisconnectedAsync_Should_Remove_User_From_Server()
    {
        var serverId = Guid.NewGuid();
        var connectionId = "connection-1";
        var server = new Server(serverId, "Test Server");
        server.ConnectedUsers.Add(new(connectionId, "TestUser"));

        _chatServiceMock.Setup(s => s.Disconnect(connectionId)).Callback(() =>
        {
            server.ConnectedUsers.RemoveAll(u => u.ConnectionId == connectionId);
        });

        await _chatHub.OnDisconnectedAsync(null);

        Assert.That(server.ConnectedUsers.Any(u => u.ConnectionId == connectionId), Is.False);
    }
}