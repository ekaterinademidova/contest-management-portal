namespace ContestService.Application.DTOs.EventStageCriteria;

public class CreateEventStageCriteriaRequest
{
    public int EventStageId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime? IsActiveUntil { get; set; }
    public bool IsRequired { get; set; }
    public decimal? MinimumThresholdScore { get; set; }
    public string? Description { get; set; }
}

