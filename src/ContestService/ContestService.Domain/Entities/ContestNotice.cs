namespace ContestService.Domain.Entities;

public class ContestNotice
{
    public int Id { get; set; }
    public int OrganizerId { get; set; }
    public int EventId { get; set; }
    public string? Goal { get; set; }
    public string? CompetitionType { get; set; }
    public string? Description { get; set; }

    // Navigation properties
    public Event? Event { get; set; }
    public ICollection<ContestDocsPackage> ContestDocsPackages { get; set; } = new List<ContestDocsPackage>();
}

