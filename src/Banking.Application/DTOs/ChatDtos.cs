namespace Banking.Application.DTOs;

public record ChatRequestDto(Guid CustomerId, string Message, Guid? AccountId, string? ConversationId);

public record ChatResponseDto(
    string ConversationId,
    string Answer,
    IReadOnlyList<string> SourcesUsed,
    IReadOnlyList<string> ToolsInvoked,
    IReadOnlyList<string> SuggestedNextActions,
    string Confidence,
    bool RequiresHumanConfirmation);
