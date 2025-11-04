using SubmissionService.Domain.Entities;

namespace SubmissionService.Domain.Interfaces;

public interface IAppealRepository : IRepository<Appeal>
{
    Task<Appeal?> GetBySubmissionIdAsync(int submissionId, CancellationToken cancellationToken = default);
}

