using Banking.Application.Interfaces.Repositories;
using Banking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Pgvector;
using Pgvector.EntityFrameworkCore;

namespace Banking.Infrastructure.Repositories;

public class KnowledgeBaseRepository(AppDbContext db) : IKnowledgeBaseRepository
{
    public async Task<IReadOnlyList<(string ChunkText, double Distance)>> SearchSimilarAsync(float[] queryEmbedding, int topK)
    {
        var vector = new Vector(queryEmbedding);

        var results = await db.KnowledgeBaseEmbeddings
            .OrderBy(e => e.Embedding.CosineDistance(vector))
            .Take(topK)
            .Select(e => new { e.ChunkText, Distance = e.Embedding.CosineDistance(vector) })
            .ToListAsync();

        return results.Select(r => (r.ChunkText, (double)r.Distance)).ToList();
    }
}
