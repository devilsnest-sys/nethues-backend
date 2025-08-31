using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ArithmeticChat.Interfaces;
using ArithmeticChat.DTOs;
using System.Security.Claims;

namespace ArithmeticChat.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChatController : ControllerBase
{
    private readonly IChatService _chat;
    public ChatController(IChatService chat) { _chat = chat; }

    private int GetUserId()
    {
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(id, out var uid) ? uid : throw new UnauthorizedAccessException();
    }

    [HttpPost("evaluate")]

    public async Task<ActionResult<ChatResponseDto>> Evaluate([FromBody] ChatRequestDto dto, CancellationToken ct)
    {
        var res = await _chat.EvaluateAsync(GetUserId(), dto, ct);
        return Ok(res);
    }

    [HttpGet("history")]
    public async Task<ActionResult<IEnumerable<ChatHistoryDto>>> History([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
    {
        var rows = await _chat.GetHistoryAsync(GetUserId(), page, pageSize, ct);
        return Ok(rows);
    }
}
