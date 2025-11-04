namespace SubmissionService.Application.DTOs.Appeal;

public class UpdateAppealRequest
{
    public int Id { get; set; }
    public int SubmissionId { get; set; }
    public string? CoverLetter { get; set; }
    public string? AppealState { get; set; }
    public string? Comment { get; set; }
}

