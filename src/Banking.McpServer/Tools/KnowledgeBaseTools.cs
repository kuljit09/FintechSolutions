using System.ComponentModel;
using Banking.Application.Interfaces.Services;

namespace Banking.McpServer.Tools;

/// <summary>ToolRiskTier.ReadOnly.</summary>
public class KnowledgeBaseTools(IKnowledgeBaseService knowledgeBaseService)
{
    [Description("Searches banking policy articles (disputes, overdraft/fees, loan eligibility, fraud protection, card policies) using semantic vector search.")]
    public async Task<IReadOnlyList<string>> SearchKnowledgeBase(string query, int topK = 3)
        => await knowledgeBaseService.SemanticSearchAsync(query, topK);
}
