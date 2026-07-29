using System.Text;
using Banking.AI.Memory;
using Banking.AI.Orchestration.PromptTemplates;
using Banking.Application.DTOs;
using Banking.Application.Interfaces.Services;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace Banking.AI.Orchestration;

/// <summary>
/// End-to-end RAG + tool-calling + human-confirmation flow. The confirmation gate runs BEFORE
/// any LLM call on a turn, so a customer's "yes" is handled deterministically by host code, not
/// reinterpreted by the model.
/// </summary>
public class ChatOrchestrator(
    Kernel kernel,
    IKnowledgeBaseService knowledgeBase,
    ChatHistoryStore historyStore,
    ConversationContext conversationContext,
    PendingConfirmationStore pendingConfirmations,
    HighRiskActionExecutor highRiskExecutor) : IChatOrchestrator
{
    public async Task<ChatResponseDto> HandleAsync(ChatRequestDto request)
    {
        var conversationId = string.IsNullOrWhiteSpace(request.ConversationId)
            ? Guid.NewGuid().ToString()
            : request.ConversationId!;

        conversationContext.ConversationId = conversationId;

        var history = historyStore.GetOrCreate(conversationId);
        history.AddUserMessage(request.Message);

        // ---- Step 1: staged high-risk action confirmation gate (deterministic, no LLM) ----
        var staged = pendingConfirmations.GetStaged(conversationId);
        if (staged is not null && ConfirmationDetector.LooksLikeConfirmation(request.Message))
        {
            var executionResult = await highRiskExecutor.ExecuteAsync(staged.ToolName, staged.ArgsJson);
            pendingConfirmations.Clear(conversationId);

            history.AddAssistantMessage(executionResult);
            historyStore.Save(conversationId, history);

            return new ChatResponseDto(
                ConversationId: conversationId,
                Answer: executionResult,
                SourcesUsed: Array.Empty<string>(),
                ToolsInvoked: new[] { staged.ToolName },
                SuggestedNextActions: Array.Empty<string>(),
                Confidence: "grounded",
                RequiresHumanConfirmation: false);
        }

        // ---- Step 2: normal RAG + Semantic Kernel tool-calling flow ----
        var retrievedChunks = await knowledgeBase.SemanticSearchAsync(request.Message);

        var executionSettings = new PromptExecutionSettings
        {
            // NOTE: verify FunctionChoiceBehavior usage against your installed SK + Ollama connector version.
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        };

        var args = new KernelArguments(executionSettings)
        {
            ["retrievedKnowledgeBaseChunks"] = retrievedChunks.Count > 0
                ? string.Join("\n---\n", retrievedChunks)
                : "(no matching knowledge base articles found)",
            ["toolResults"] = "(tool results are injected automatically by Semantic Kernel's function-calling loop)",
            ["chatHistory"] = RenderHistory(history),
            ["userMessage"] = request.Message
        };

        var invokedFunctions = new List<string>();
        kernel.FunctionInvoked += (_, e) => invokedFunctions.Add(e.Function.Name);

        var result = await kernel.InvokePromptAsync(GroundingPrompt.Template, args);
        var answer = result.ToString();

        history.AddAssistantMessage(answer);
        historyStore.Save(conversationId, history);

        // If this turn just staged a high-risk action (e.g. request_block_card ran), flag it so
        // Angular can visually mark the message as "awaiting your confirmation".
        var requiresConfirmation = pendingConfirmations.GetStaged(conversationId) is not null;

        return new ChatResponseDto(
            ConversationId: conversationId,
            Answer: answer,
            SourcesUsed: retrievedChunks,
            ToolsInvoked: invokedFunctions,
            SuggestedNextActions: DeriveNextActions(invokedFunctions, requiresConfirmation),
            Confidence: retrievedChunks.Count > 0 || invokedFunctions.Count > 0 ? "grounded" : "low-context",
            RequiresHumanConfirmation: requiresConfirmation);
    }

    private static string RenderHistory(ChatHistory history)
    {
        var sb = new StringBuilder();
        foreach (var msg in history.TakeLast(6))
            sb.AppendLine($"{msg.Role}: {msg.Content}");
        return sb.ToString();
    }

    private static IReadOnlyList<string> DeriveNextActions(IReadOnlyList<string> toolsInvoked, bool requiresConfirmation)
    {
        var actions = new List<string>();
        if (requiresConfirmation) actions.Add("Confirm the pending action");
        if (toolsInvoked.Contains("check_dispute_eligibility")) actions.Add("File a dispute");
        if (toolsInvoked.Contains("explain_transaction_failure")) actions.Add("Retry the payment");
        if (toolsInvoked.Count == 0 && !requiresConfirmation) actions.Add("Create a support ticket");
        return actions;
    }
}
