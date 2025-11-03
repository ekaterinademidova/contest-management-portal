using ContestService.Application.DTOs.ContestNotice;

namespace ContestService.Application.Services;

public interface IContestNoticeService
{
    Task<ContestNoticeDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ContestNoticeDto>> GetAllAsync(ContestNoticeFilterRequest? filter = null, CancellationToken cancellationToken = default);
    Task<ContestNoticeDto> CreateAsync(CreateContestNoticeRequest request, CancellationToken cancellationToken = default);
    Task<ContestNoticeDto> UpdateAsync(UpdateContestNoticeRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}

