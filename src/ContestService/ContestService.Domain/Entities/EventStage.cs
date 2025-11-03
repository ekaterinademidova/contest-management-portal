namespace ContestService.Domain.Entities;

public class EventStage
{
    public int Id { get; set; }
    public int? PreviousStageId { get; set; }
    public int EventId { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    // Navigation properties
    public Event? Event { get; set; }
    public EventStage? PreviousStage { get; set; }
    public ICollection<EventStage> NextStages { get; set; } = new List<EventStage>();
    public ICollection<EventStageCriteria> EventStageCriterias { get; set; } = new List<EventStageCriteria>();
}

