namespace Banking.AI.McpClient;

/// <summary>
/// Same abstraction as the e-commerce project - transport-agnostic wrapper over a real MCP
/// client connection to Banking.McpServer.
/// </summary>
public interface IMcpToolClient
{
    Task<TResult?> CallToolAsync<TResult>(string toolName, object arguments);
}
