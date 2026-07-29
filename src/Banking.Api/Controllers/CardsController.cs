using Banking.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Banking.Api.Controllers;

[ApiController]
[Route("api/cards")]
public class CardsController(ICardService cardService) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var card = await cardService.GetCardStatusAsync(id);
        return card is null ? NotFound() : Ok(card);
    }

    /// <summary>
    /// Direct REST path to block a card - deliberately NOT confirmation-gated the way the
    /// chatbot path is, because here the confirmation IS the explicit button press/API call
    /// itself (the Angular UI shows its own "Are you sure?" dialog before calling this). This
    /// is a good interview point: the guardrail belongs at the boundary where an autonomous
    /// agent might act on ambiguous natural language, not on every code path that reaches the
    /// same operation - a human clicking a confirm button IS the confirmation.
    /// </summary>
    [HttpPost("{id:guid}/block")]
    public async Task<IActionResult> Block(Guid id, [FromBody] BlockCardBody body)
        => Ok(await cardService.BlockCardAsync(id, body.Reason));

    public record BlockCardBody(string Reason);
}
