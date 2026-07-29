using System.Net.Http.Json;
using Banking.Application.Interfaces.Services;

namespace Banking.Infrastructure.VectorSearch;

public class OllamaEmbeddingGenerator(HttpClient http) : IEmbeddingGenerator
{
    private const string Model = "nomic-embed-text";

    public async Task<float[]> GenerateAsync(string text)
    {
        var response = await http.PostAsJsonAsync("/api/embeddings", new { model = Model, prompt = text });
        response.EnsureSuccessStatusCode();
        var payload = await response.Content.ReadFromJsonAsync<OllamaEmbeddingResponse>();
        return payload?.Embedding ?? throw new InvalidOperationException("Ollama returned no embedding.");
    }

    private class OllamaEmbeddingResponse
    {
        public float[]? Embedding { get; set; }
    }
}
