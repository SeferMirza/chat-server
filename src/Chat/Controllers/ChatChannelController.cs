using Chat.Models;
using Chat.Services;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Controllers;

[ApiController]
[Route("chat")]
public partial class ChatChannelController([FromKeyedServices(nameof(ChatChannelService))] IService _chatService) : ControllerBase
{
    [HttpPost("create-server")]
    public APIResponse<ServerDetail> CreateServer([FromQuery] string name)
    {
        var server = _chatService.CreateServer(name);
        ServerDetail result = new(
            server.ServerId,
            server.ServerName,
            server.Capacity,
            []
        );

        return APIResponse<ServerDetail>.SuccessResponse(data: result);
    }

    [HttpGet("servers")]
    public APIResponse<List<ServerInfo>> GetServers()
    {
        var servers = _chatService.GetServers();

        return APIResponse<List<ServerInfo>>.SuccessResponse(data: servers);
    }

    [HttpGet("username-is-valid")]
    public APIResponse<bool> CheckUsername([FromQuery] Guid serverId, [FromQuery] string username)
    {
        var response = _chatService.CheckUsername(serverId, username);

        return APIResponse<bool>.SuccessResponse(data: response);
    }

    [HttpGet("server-detail")]
    public APIResponse<ServerDetail> GetServerDetail([FromQuery] Guid id)
    {
        var server = _chatService.GetServer(id);

        ServerDetail result = new(
            server.ServerId,
            server.ServerName,
            server.Capacity,
            server.ConnectedUsers.Select(user => user.Name).ToList()
        );

        return APIResponse<ServerDetail>.SuccessResponse(data: result);
    }
}
