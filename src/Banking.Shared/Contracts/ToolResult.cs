namespace Banking.Shared.Contracts;

public class ToolResult<T>
{
    public bool Success { get; init; }
    public T? Data { get; init; }
    public string? ErrorReason { get; init; }

    public static ToolResult<T> Ok(T data) => new() { Success = true, Data = data };
    public static ToolResult<T> Fail(string reason) => new() { Success = false, ErrorReason = reason };
}
