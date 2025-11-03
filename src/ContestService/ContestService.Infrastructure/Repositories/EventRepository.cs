using ContestService.Domain.Entities;
using ContestService.Domain.Interfaces;
using ContestService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ContestService.Infrastructure.Repositories;

public class EventRepository : BaseRepository<Event>, IEventRepository
{
    public EventRepository(ContestDbContext context) : base(context)
    {
    }

    public override async Task<Event?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(e => e.EventStages)
            .Include(e => e.ContestNotices)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }
}

