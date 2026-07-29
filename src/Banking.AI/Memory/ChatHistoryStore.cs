using System.Collections.Concurrent;
using Microsoft.SemanticKernel.ChatCompletion;

namespace Banking.AI.Memory;

public class ChatHistoryStore
{
    private readonly ConcurrentDictionary<string, ChatHistory> _histories = new();

    public ChatHistory GetOrCreate(string conversationId) => _histories.GetOrAdd(conversationId, _ => new ChatHistory());
    public void Save(string conversationId, ChatHistory history) => _histories[conversationId] = history;
}
