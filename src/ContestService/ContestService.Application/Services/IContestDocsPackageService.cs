using ContestService.Application.DTOs.ContestDocsPackage;

namespace ContestService.Application.Services;

public interface IContestDocsPackageService
{
    Task<ContestDocsPackageDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ContestDocsPackageDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ContestDocsPackageDto>> GetByContestNoticeIdAsync(int contestNoticeId, CancellationToken cancellationToken = default);
    Task<ContestDocsPackageDto> CreateAsync(CreateContestDocsPackageRequest request, CancellationToken cancellationToken = default);
    Task<ContestDocsPackageDto> UpdateAsync(UpdateContestDocsPackageRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}

