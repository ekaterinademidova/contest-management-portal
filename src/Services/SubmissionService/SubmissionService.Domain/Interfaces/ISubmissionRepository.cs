using SubmissionService.Domain.Entities;

namespace SubmissionService.Domain.Interfaces;

public interface ISubmissionRepository : IRepository<Submission>
{
    Task<IEnumerable<Submission>> GetByContestNoticeIdAsync(int contestNoticeId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Submission>> GetByParticipantIdAsync(int participantId, CancellationToken cancellationToken = default);
}

