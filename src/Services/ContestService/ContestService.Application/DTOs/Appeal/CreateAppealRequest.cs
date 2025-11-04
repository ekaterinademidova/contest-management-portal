namespace ContestService.Application.DTOs.Appeal;

public class CreateAppealRequest
{
    public int SubmissionId { get; set; }
    public string? CoverLetter { get; set; }
    public string? AppealState { get; set; }
    public string? Comment { get; set; }
}

