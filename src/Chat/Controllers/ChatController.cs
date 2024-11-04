using Chat.Models;
using Chat.Services;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Controllers;

[ApiController]
[Route("[controller]")]
public partial class ChatController(IChatService _chatService) : ControllerBase
{
    [HttpPost("create-server")]
    public ActionResult<Server> CreateServer([FromQuery] string name)
    {
        var server = _chatService.CreateServer(name);

        return Ok(server);
    }
    [HttpGet("servers")]
    public ActionResult<List<ServerInfo>> GetServers()
    {
        var server = _chatService.GetServers();

        return Ok(server);
    }
}
