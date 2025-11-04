namespace ContestService.Application.DTOs.Submission;

public class SubmissionFilterRequest
{
    public int? ContestNoticeId { get; set; }
    public int? ParticipantId { get; set; }
    public string? SubmissionState { get; set; }
}

