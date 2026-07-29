# Banking / Fintech Support — GenAI Monolith on .NET 8

Same architecture as the e-commerce project (Domain → Application → Infrastructure → AI →
MCP Server → Api, Angular 20+ frontend, Semantic Kernel + Ollama + PostgreSQL/pgvector),
rebuilt for a **banking customer support** domain. This is the more advanced of the two
projects — it adds three concepts the e-commerce version deliberately kept simple:

1. **Multiple, risk-tiered write tools** (`ToolRiskTier.ReadOnly` / `LowRiskWrite` /
   `HighRiskWrite`) instead of a single write exception.
2. **A human-in-the-loop confirmation gate** for the one `HighRiskWrite` tool (`BlockCard`) —
   the LLM can *stage* a card block, but only explicit customer confirmation in the next
   conversation turn triggers the real action, and that trigger is host code, not the model.
3. **A "near real-time" background fraud-detection sweep** (`FraudSweepBackgroundService`)
   that continuously re-evaluates new transactions independent of any chat request — the
   chatbot's fraud-alert answers only ever *read* what this sweep already wrote.

## What's here

```
BankingFinTechSupport.sln
src/
  Banking.Domain/          Entities, enums, OverdraftPolicy, DisputeEligibilityPolicy,
                            LoanEligibilityPolicy, FraudRiskPolicy
  Banking.Shared/          ToolResult<T>, ToolRiskTier, constants
  Banking.Application/     DTOs, repository + service interfaces, service implementations
  Banking.Infrastructure/  EF Core DbContext, configs, repositories, Ollama embedding client,
                            FraudSweepBackgroundService
  Banking.AI/              Semantic Kernel setup, plugins, confirmation-gate memory classes
                            (ConversationContext, PendingConfirmationStore, ConfirmationDetector),
                            HighRiskActionExecutor, ChatOrchestrator
  Banking.McpServer/       MCP tool classes (Account/Transaction/Card/Loan/Dispute/FraudAlert/
                            KnowledgeBase/SupportTicket)
  Banking.Api/             Controllers + Program.cs
db/
  seed.sql                 DATA ONLY - see below, no schema/extension SQL
frontend/banking-ui/       Angular 20+ (Accounts, Transactions, Loans, Cards, Support Chat)
```

## Domain model at a glance

`Customer` → `Account` → `Transaction` / `Card`, `Customer` → `Loan` → `LoanRepayment`,
`Transaction` → `Dispute`, `Account` → `FraudAlert`, `Customer`/`Account` → `SupportTicket`,
plus `KnowledgeBaseArticle` (+ pgvector `KnowledgeBaseEmbedding`) for RAG.

## The chatbot scenarios this is built to answer

- "Why did my transaction fail?" → `ExplainTransactionFailure`, grounded in a **live
  re-evaluation** of `OverdraftPolicy` against the account's current balance, not a stale stored string.
- "Can I dispute this charge?" / "Dispute it" → `CheckDisputeEligibility` (grounded in
  `DisputeEligibilityPolicy`: completed, within 60 days, not already disputed) → `FileDispute`
  (executes immediately — low risk, reversible, non-monetary at filing time).
- "Is my loan approved?" / "Why was it rejected?" → `GetLoanStatus`, grounded in
  `LoanEligibilityPolicy` (credit score threshold + income-based cap).
- "Block my card, I lost it" → `request_block_card` **stages** the action and asks the customer
  to confirm; only a following "yes" actually blocks it, via `HighRiskActionExecutor`.
- "Why was there a fraud alert on my account?" → `GetFraudAlerts` reads alerts the background
  sweep already raised via `FraudRiskPolicy` — the chat turn never runs fraud detection itself.
- "What's your dispute/overdraft/loan/fraud/card policy?" → pgvector RAG over
  `knowledge_base_articles`.
- Anything unresolved → `CreateSupportTicket` (low-risk write).

## Seed data — your local setup, per your instructions

You already have pgAdmin + pgvector installed locally, so **this project does not ship
extension/schema/index SQL files** the way the e-commerce project did. Instead:

1. Create the schema with EF Core migrations (schema comes from `Banking.Infrastructure`'s
   `AppDbContext` + entity configurations — the enum-to-string conversion is applied centrally
   via reflection in `AppDbContext.OnModelCreating`, so you won't see per-entity enum config
   scattered around):
   ```bash
   cd src/Banking.Api
   dotnet tool install --global dotnet-ef   # if not already installed
   dotnet ef migrations add InitialCreate --project ../Banking.Infrastructure --startup-project .
   dotnet ef database update --project ../Banking.Infrastructure --startup-project .
   ```
2. Load sample data:
   ```bash
   psql -h localhost -U postgres -d banking_genai -f ../../db/seed.sql
   ```
3. Generate real KB embeddings (same pattern as the e-commerce project — see the note at the
   bottom of `seed.sql` and `EmbeddingIngestionHelper` in `Banking.Infrastructure/VectorSearch`),
   then create the similarity index once real vectors exist:
   ```sql
   CREATE INDEX ON knowledge_base_embeddings USING ivfflat (embedding vector_cosine_ops) WITH (lists = 100);
   ```

## Local setup (rest of the flow, same shape as the e-commerce project)

```bash
ollama pull llama3.1
ollama pull nomic-embed-text
ollama serve

dotnet restore
dotnet build

dotnet run --project src/Banking.Api          # http://localhost:5100 (set in appsettings.json)
dotnet run --project src/Banking.McpServer     # separate process

cd frontend/banking-ui
npm install
ng serve                                       # http://localhost:4200
```

**Note on the fraud sweep running twice:** both `Banking.Api` and `Banking.McpServer` call
`AddInfrastructure()`, which registers `FraudSweepBackgroundService` as a hosted service. That's
fine for local learning (you'll just see the sweep log from both processes), but in a real
deployment run it in exactly one place — flagged with a comment in `Banking.McpServer/Program.cs`.

## Verify before you build — same prerelease-API caveats as the e-commerce project

Identical list of spots to check package versions/API signatures against what you actually
restore: `Microsoft.SemanticKernel.Connectors.Ollama` (Banking.AI.csproj), `AddOllamaChatCompletion`
(`KernelBuilderExtensions.cs`), `FunctionChoiceBehavior` (`ChatOrchestrator.cs`), the
`ModelContextProtocol` SDK's server registration (`Banking.McpServer/Program.cs`, currently
pseudocode/commented) and client (`Banking.AI/McpClient/McpToolClient.cs`, currently throws
`NotImplementedException`), and `UseVector()` from `Pgvector.EntityFrameworkCore`
(`Banking.Infrastructure/DependencyInjection.cs`).

## The confirmation-gate pattern, if you want to explain it in an interview

> "Blocking a card is security-critical, so I didn't let the model trigger it directly. When
> the LLM decides to call `request_block_card`, that plugin method doesn't touch the database —
> it stages the request in a `PendingConfirmationStore` keyed by conversation id, using an
> `AsyncLocal`-backed `ConversationContext` so the conversation identity never has to pass
> through the model as a parameter. The orchestrator checks for a staged action *before* every
> LLM call; if the customer's next message looks like a confirmation, host code — not the model —
> calls `HighRiskActionExecutor`, which invokes the real `ICardService.BlockCardAsync`. The model
> never gets a second chance to reinterpret 'yes' as something else, because it's never asked."

This is the single most interview-differentiating piece of this project versus the e-commerce
one — it demonstrates you understand agentic AI needs *deterministic, host-owned guardrails*
around consequential actions, not just prompt-level instructions telling the model to be careful.
