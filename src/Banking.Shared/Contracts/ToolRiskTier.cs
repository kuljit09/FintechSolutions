namespace Banking.Shared.Contracts;

/// <summary>
/// Banking exposes MORE than one write-capable tool (unlike the e-commerce project's single
/// CreateSupportTicket exception), so tools are explicitly tiered by risk. This tiering is the
/// interview-worthy addition over the e-commerce project: not all AI-agent actions carry equal
/// blast radius, and the MCP surface should say so explicitly rather than treating every write
/// tool the same way.
/// </summary>
public enum ToolRiskTier
{
    /// <summary>Read-only. No confirmation needed. e.g. GetAccountBalance.</summary>
    ReadOnly,

    /// <summary>Writes non-monetary, reversible state. e.g. CreateSupportTicket, FileDispute.</summary>
    LowRiskWrite,

    /// <summary>Security-critical or affects account access. e.g. BlockCard. Should require
    /// explicit user confirmation in the conversation before the agent invokes it, and should
    /// be logged/audited distinctly from LowRiskWrite tools.</summary>
    HighRiskWrite
}
