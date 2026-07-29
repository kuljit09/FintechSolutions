namespace Banking.Application.Interfaces.Services;

public interface IKnowledgeBaseService
{
    Task<IReadOnlyList<string>> SemanticSearchAsync(string query, int topK = 3);
}

public interface IEmbeddingGenerator
{
    Task<float[]> GenerateAsync(string text);
}
