namespace ContestService.Domain.Entities;

public class EventStageCriteria
{
    public int Id { get; set; }
    public int EventStageId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime? IsActiveUntil { get; set; }
    public bool IsRequired { get; set; }
    public decimal? MinimumThresholdScore { get; set; }
    public string? Description { get; set; }

    // Navigation properties
    public EventStage? EventStage { get; set; }
}

