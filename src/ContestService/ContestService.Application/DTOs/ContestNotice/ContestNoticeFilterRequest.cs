namespace ContestService.Application.DTOs.ContestNotice;

public class ContestNoticeFilterRequest
{
    public int? OrganizerId { get; set; }
    public int? EventId { get; set; }
    public string? CompetitionType { get; set; }
}

