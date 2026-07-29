namespace Banking.Domain.Entities;

public class KnowledgeBaseArticle
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;
    public string Category { get; set; } = default!;
}
