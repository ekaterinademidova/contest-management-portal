namespace ContestService.Application.DTOs.ContestNotice;

public class CreateContestNoticeRequest
{
    public int OrganizerId { get; set; }
    public int EventId { get; set; }
    public string? Goal { get; set; }
    public string? CompetitionType { get; set; }
    public string? Description { get; set; }
}

