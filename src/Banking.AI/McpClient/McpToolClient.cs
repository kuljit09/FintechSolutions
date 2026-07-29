namespace Banking.AI.McpClient;

/// <summary>
/// PLACEHOLDER - see the equivalent file in the e-commerce project's ECommerce.AI for the same
/// caveat: the official ModelContextProtocol C# SDK's client API is still prerelease. Wire this
/// up to that SDK's real stdio/HTTP client once you've pinned a version - not guessing at the
/// exact call shape here.
/// </summary>
public class McpToolClient : IMcpToolClient
{
    public Task<TResult?> CallToolAsync<TResult>(string toolName, object arguments)
    {
        throw new NotImplementedException(
            $"Wire this up to the ModelContextProtocol C# SDK's client for tool '{toolName}'. " +
            "See Banking.McpServer for the corresponding tool definitions.");
    }
}
