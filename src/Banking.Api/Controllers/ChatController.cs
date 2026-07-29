using Banking.AI.Orchestration;
using Banking.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Banking.Api.Controllers;

[ApiController]
[Route("api/chat")]
public class ChatController(IChatOrchestrator chatOrchestrator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ChatResponseDto>> Post([FromBody] ChatRequestDto request)
        => Ok(await chatOrchestrator.HandleAsync(request));
}
