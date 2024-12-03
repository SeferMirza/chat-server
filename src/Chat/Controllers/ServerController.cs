using Chat.Models;
using Chat.Services;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Controllers;

[ApiController]
[Route("server")]
public partial class ServerController(IService _chatService) : ControllerBase
{
    [HttpPost("create-server")]
    public APIResponse<ServerDetailInfo> CreateServer([FromQuery] string name, [FromQuery] ServerType serverType = ServerType.Chat, [FromQuery] bool isPublic = true)
    {
        var server = _chatService.CreateServer(name, serverType, isPublic);
        ServerDetailInfo result = new()
        {
            ServerId = server.ServerId,
            ServerName = server.ServerName,
            ServerType = server.ServerType,
            Capacity = server.Capacity,
            Public = server.Public
        };

        return APIResponse<ServerDetailInfo>.SuccessResponse(data: result);
    }

    [HttpGet("servers")]
    public APIResponse<List<ServerDetailInfo>> GetServers()
    {
        var servers = _chatService.GetServers();

        return APIResponse<List<ServerDetailInfo>>.SuccessResponse(data: servers);
    }

    [HttpGet("server-detail")]
    public APIResponse<ServerDetailInfo> GetServerDetail([FromQuery] Guid id)
    {
        var server = _chatService.GetServer(id);

        ServerDetailInfo result = new()
        {
            ServerId = server.ServerId,
            ServerName = server.ServerName,
            Capacity = server.Capacity,
            Public = server.Public,
            ConnectedUsers = server.ConnectedUsers
        };

        return APIResponse<ServerDetailInfo>.SuccessResponse(data: result);
    }
}
