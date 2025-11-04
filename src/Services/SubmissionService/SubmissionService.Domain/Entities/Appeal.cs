namespace SubmissionService.Domain.Entities;

public class Appeal
{
    public int Id { get; set; }
    public int SubmissionId { get; set; }
    public DateTime DateTime { get; set; }
    public string? CoverLetter { get; set; }
    public string? AppealState { get; set; }
    public string? Comment { get; set; }

    // Navigation properties
    public Submission? Submission { get; set; }
}

