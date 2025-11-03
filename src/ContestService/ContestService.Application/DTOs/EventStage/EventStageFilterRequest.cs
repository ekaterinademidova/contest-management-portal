namespace ContestService.Application.DTOs.EventStage;

public class EventStageFilterRequest
{
    public int? EventId { get; set; }
    public int? PreviousStageId { get; set; }
    public string? Name { get; set; }
}

