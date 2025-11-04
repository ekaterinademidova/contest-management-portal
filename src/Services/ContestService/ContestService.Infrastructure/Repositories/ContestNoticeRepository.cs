using ContestService.Domain.Entities;
using ContestService.Domain.Interfaces;
using ContestService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ContestService.Infrastructure.Repositories;

public class ContestNoticeRepository : BaseRepository<ContestNotice>, IContestNoticeRepository
{
    public ContestNoticeRepository(ContestDbContext context) : base(context)
    {
    }

    public override async Task<ContestNotice?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Event)
            .Include(c => c.ContestDocsPackages)
                .ThenInclude(c => c.Attachment)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }
}

