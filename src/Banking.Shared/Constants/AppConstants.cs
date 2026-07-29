namespace Banking.Shared.Constants;

public static class AppConstants
{
    public const int DisputeWindowDays = 60;
    public const int MaxTransactionsReturnedToAgent = 15;
    public const int DefaultVectorSearchTopK = 3;
    public const int EmbeddingDimensions = 768; // must match nomic-embed-text

    public static class OllamaModels
    {
        public const string ChatModel = "llama3.1";
        public const string EmbeddingModel = "nomic-embed-text";
    }

    public static class FraudSweep
    {
        public const int IntervalSeconds = 30; // "near real-time" background sweep cadence
    }
}
