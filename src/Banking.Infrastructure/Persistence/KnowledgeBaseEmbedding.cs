using Pgvector;

namespace Banking.Infrastructure.Persistence;

public class KnowledgeBaseEmbedding
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ArticleId { get; set; }
    public string ChunkText { get; set; } = default!;
    public Vector Embedding { get; set; } = default!;
}
