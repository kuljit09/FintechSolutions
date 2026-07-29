namespace Banking.Infrastructure.VectorSearch;

public static class EmbeddingIngestionHelper
{
    public static IEnumerable<string> ChunkText(string content, int approxWordsPerChunk = 120)
    {
        var words = content.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < words.Length; i += approxWordsPerChunk)
            yield return string.Join(' ', words.Skip(i).Take(approxWordsPerChunk));
    }
}
