namespace ContestService.Application.DTOs.EventStage;

public class CreateEventStageRequest
{
    public int? PreviousStageId { get; set; }
    public int EventId { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

