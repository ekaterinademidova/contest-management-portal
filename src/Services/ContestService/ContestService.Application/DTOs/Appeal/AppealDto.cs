namespace ContestService.Application.DTOs.Appeal;

public class AppealDto
{
    public int Id { get; set; }
    public int SubmissionId { get; set; }
    public DateTime DateTime { get; set; }
    public string? CoverLetter { get; set; }
    public string? AppealState { get; set; }
    public string? Comment { get; set; }
}

