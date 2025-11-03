namespace ContestService.Domain.Entities;

public class Event
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    // Navigation properties
    public ICollection<EventStage> EventStages { get; set; } = new List<EventStage>();
    public ICollection<ContestNotice> ContestNotices { get; set; } = new List<ContestNotice>();
}

