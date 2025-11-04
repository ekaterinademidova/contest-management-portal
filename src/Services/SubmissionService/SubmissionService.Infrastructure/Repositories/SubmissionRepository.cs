using Microsoft.EntityFrameworkCore;
using SubmissionService.Domain.Entities;
using SubmissionService.Domain.Interfaces;
using SubmissionService.Infrastructure.Data;

namespace SubmissionService.Infrastructure.Repositories;

public class SubmissionRepository : BaseRepository<Submission>, ISubmissionRepository
{
    public SubmissionRepository(SubmissionDbContext context) : base(context)
    {
    }

    public override async Task<Submission?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.Appeal)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Submission>> GetByContestNoticeIdAsync(int contestNoticeId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.Appeal)
            .Where(s => s.ContestNoticeId == contestNoticeId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Submission>> GetByParticipantIdAsync(int participantId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.Appeal)
            .Where(s => s.ParticipantId == participantId)
            .ToListAsync(cancellationToken);
    }
}

