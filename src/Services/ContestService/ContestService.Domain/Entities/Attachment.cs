namespace ContestService.Domain.Entities;

public class Attachment
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<ContestDocsPackage> ContestDocsPackages { get; set; } = new List<ContestDocsPackage>();
}

