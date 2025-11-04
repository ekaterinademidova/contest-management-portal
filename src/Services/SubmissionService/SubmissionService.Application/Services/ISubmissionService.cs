using SubmissionService.Application.DTOs.Appeal;
using SubmissionService.Application.DTOs.Submission;

namespace SubmissionService.Application.Services;

public interface ISubmissionService
{
    // Submission methods
    Task<SubmissionDto> GetSubmissionByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<SubmissionDto>> GetAllSubmissionsAsync(SubmissionFilterRequest? filter = null, CancellationToken cancellationToken = default);
    Task<SubmissionDto> CreateSubmissionAsync(CreateSubmissionRequest request, CancellationToken cancellationToken = default);
    Task<SubmissionDto> UpdateSubmissionAsync(UpdateSubmissionRequest request, CancellationToken cancellationToken = default);
    Task DeleteSubmissionAsync(int id, CancellationToken cancellationToken = default);

    // Appeal methods
    Task<AppealDto> GetAppealByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<AppealDto?> GetAppealBySubmissionIdAsync(int submissionId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AppealDto>> GetAllAppealsAsync(CancellationToken cancellationToken = default);
    Task<AppealDto> CreateAppealAsync(CreateAppealRequest request, CancellationToken cancellationToken = default);
    Task<AppealDto> UpdateAppealAsync(UpdateAppealRequest request, CancellationToken cancellationToken = default);
    Task DeleteAppealAsync(int id, CancellationToken cancellationToken = default);
}

