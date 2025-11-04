namespace ContestService.Domain.Entities;

public class ContestDocsPackage
{
    public int Id { get; set; }
    public int ContestNoticeId { get; set; }
    public int AttachmentId { get; set; }
    public bool IsRequired { get; set; }
    public string? Comment { get; set; }

    // Navigation properties
    public ContestNotice? ContestNotice { get; set; }
    public Attachment? Attachment { get; set; }
}

