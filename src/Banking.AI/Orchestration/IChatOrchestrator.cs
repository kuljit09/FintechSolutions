using Banking.Application.DTOs;

namespace Banking.AI.Orchestration;

public interface IChatOrchestrator
{
    Task<ChatResponseDto> HandleAsync(ChatRequestDto request);
}
