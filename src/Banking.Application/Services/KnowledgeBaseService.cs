using Banking.Application.Interfaces.Repositories;
using Banking.Application.Interfaces.Services;

namespace Banking.Application.Services;

public class KnowledgeBaseService(IEmbeddingGenerator embeddings, IKnowledgeBaseRepository kb) : IKnowledgeBaseService
{
    public async Task<IReadOnlyList<string>> SemanticSearchAsync(string query, int topK = 3)
    {
        var queryEmbedding = await embeddings.GenerateAsync(query);
        var results = await kb.SearchSimilarAsync(queryEmbedding, topK);
        return results.Select(r => r.ChunkText).ToList();
    }
}
