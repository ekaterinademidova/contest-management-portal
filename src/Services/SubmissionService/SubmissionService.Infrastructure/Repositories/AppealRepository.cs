using Microsoft.EntityFrameworkCore;
using SubmissionService.Domain.Entities;
using SubmissionService.Domain.Interfaces;
using SubmissionService.Infrastructure.Data;

namespace SubmissionService.Infrastructure.Repositories;

public class AppealRepository : BaseRepository<Appeal>, IAppealRepository
{
    public AppealRepository(SubmissionDbContext context) : base(context)
    {
    }

    public override async Task<Appeal?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(a => a.Submission)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<Appeal?> GetBySubmissionIdAsync(int submissionId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(a => a.Submission)
            .FirstOrDefaultAsync(a => a.SubmissionId == submissionId, cancellationToken);
    }
}

