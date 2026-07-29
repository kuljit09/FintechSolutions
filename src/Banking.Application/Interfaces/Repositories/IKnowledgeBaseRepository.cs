namespace Banking.Application.Interfaces.Repositories;

public interface IKnowledgeBaseRepository
{
    Task<IReadOnlyList<(string ChunkText, double Distance)>> SearchSimilarAsync(float[] queryEmbedding, int topK);
}
