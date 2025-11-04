namespace ContestService.Application.DTOs.ContestDocsPackage;

public class UpdateContestDocsPackageRequest
{
    public int Id { get; set; }
    public int ContestNoticeId { get; set; }
    public int AttachmentId { get; set; }
    public bool IsRequired { get; set; }
    public string? Comment { get; set; }
}

