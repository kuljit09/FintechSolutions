using System.ComponentModel;
using Banking.Application.Interfaces.Services;
using Microsoft.SemanticKernel;

namespace Banking.AI.Plugins;

/// <summary>
/// Called in-process (not via MCP) - vector search is always-run retrieval context, not a
/// discrete action the model "decides" to take. Same distinction made in the e-commerce project.
/// </summary>
public class KnowledgeBasePlugin(IKnowledgeBaseService kb)
{
    [KernelFunction("search_knowledge_base")]
    [Description("Searches banking policy articles (disputes, overdraft/fees, loan eligibility, fraud protection, card policies) using semantic vector search.")]
    public async Task<IReadOnlyList<string>> SearchKnowledgeBase(
        [Description("The customer's question")] string query,
        [Description("Number of chunks to retrieve")] int topK = 3)
        => await kb.SemanticSearchAsync(query, topK);
}
