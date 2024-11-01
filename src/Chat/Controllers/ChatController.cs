using System.Security.Claims;
using Chat.DTOs;
using Chat.Models;
using Chat.Services;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Controllers;

[ApiController]
[Route("[controller]")]
public class ChatController(IChatService _chatService) : ControllerBase
{
    [HttpPost("servers")]
    public async Task<ActionResult<Server>> CreateServer(CreateServerDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var server = await _chatService.CreateServerAsync(userId, dto);
        return Ok(server);
    }

    [HttpPost("rooms")]
    public async Task<ActionResult<Room>> CreateRoom(CreateRoomDto dto)
    {
        var room = await _chatService.CreateRoomAsync(dto.ServerId, dto);
        return Ok(room);
    }
}