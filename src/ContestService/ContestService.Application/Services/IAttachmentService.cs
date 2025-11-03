using ContestService.Application.DTOs.Attachment;

namespace ContestService.Application.Services;

public interface IAttachmentService
{
    Task<AttachmentDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<AttachmentDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<AttachmentDto> CreateAsync(CreateAttachmentRequest request, CancellationToken cancellationToken = default);
    Task<AttachmentDto> UpdateAsync(UpdateAttachmentRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}

