using Chat.Models;
using Chat.Services;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Controllers;

[ApiController]
[Route("[controller]")]
public partial class ChatController(IChatService _chatService) : ControllerBase
{
    [HttpPost("create-server")]
    public ActionResult<ServerInfo> CreateServer([FromQuery] string name)
    {
        var server = _chatService.CreateServer(name);
        ServerDetail result = new(
            server.ServerId,
            server.ServerName,
            server.Capacity,
            []
        );

        return Ok(result);
    }

    [HttpGet("servers")]
    public ActionResult<List<ServerInfo>> GetServers()
    {
        var servers = _chatService.GetServers();

        return Ok(servers);
    }

    [HttpGet("username-is-valid")]
    public ActionResult<bool> CheckUsername([FromQuery] Guid serverId, [FromQuery] string username)
    {
        var response = _chatService.CheckUsername(serverId, username);

        return Ok(response);
    }

    [HttpGet("server-detail")]
    public ActionResult<List<ServerDetail>> GetServerDetail([FromQuery] Guid id)
    {
        var server = _chatService.GetServer(id);
        ServerDetail result = new(
            server.ServerId,
            server.ServerName,
            server.Capacity,
            server.ConnectedUsers.Select(user => user.Name).ToList()
        );

        return Ok(result);
    }
}
