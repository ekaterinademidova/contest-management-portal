using ContestService.Domain.Entities;
using ContestService.Domain.Interfaces;
using ContestService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ContestService.Infrastructure.Repositories;

public class ContestDocsPackageRepository : BaseRepository<ContestDocsPackage>, IContestDocsPackageRepository
{
    public ContestDocsPackageRepository(ContestDbContext context) : base(context)
    {
    }

    public override async Task<ContestDocsPackage?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.ContestNotice)
            .Include(c => c.Attachment)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }
}

