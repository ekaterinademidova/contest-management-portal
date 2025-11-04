namespace ContestService.Application.DTOs.Submission;

public class UpdateSubmissionRequest
{
    public int Id { get; set; }
    public int ContestNoticeId { get; set; }
    public int ParticipantId { get; set; }
    public string? CoverLetter { get; set; }
    public string? Comment { get; set; }
    public bool? DocsPackageIsValid { get; set; }
    public string? SubmissionState { get; set; }
}

