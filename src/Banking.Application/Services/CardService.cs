using Banking.Application.DTOs;
using Banking.Application.Interfaces.Repositories;
using Banking.Application.Interfaces.Services;
using Banking.Domain.Enums;

namespace Banking.Application.Services;

public class CardService(ICardRepository cards) : ICardService
{
    public async Task<CardDto?> GetCardStatusAsync(Guid cardId)
    {
        var c = await cards.GetByIdAsync(cardId);
        return c is null ? null : Map(c);
    }

    public async Task<BlockCardResultDto> BlockCardAsync(Guid cardId, string reason)
    {
        var card = await cards.GetByIdAsync(cardId);
        if (card is null)
            return new BlockCardResultDto(cardId, false, "Unknown", "Card not found.");

        if (card.Status == CardStatus.Blocked)
            return new BlockCardResultDto(cardId, true, card.Status.ToString(), "Card was already blocked.");

        card.Status = CardStatus.Blocked;
        card.BlockReason = reason;
        await cards.UpdateAsync(card);

        return new BlockCardResultDto(cardId, true, card.Status.ToString(), "Card has been blocked successfully. A replacement can be requested if needed.");
    }

    private static CardDto Map(Domain.Entities.Card c) =>
        new(c.Id, c.AccountId, c.MaskedNumber, c.Type.ToString(), c.Status.ToString(), c.ExpiryDate, c.DailyLimit, c.BlockReason);
}
