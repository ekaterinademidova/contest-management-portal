using ContestService.Domain.Entities;
using ContestService.Domain.Interfaces;
using ContestService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ContestService.Infrastructure.Repositories;

public class EventStageRepository : BaseRepository<EventStage>, IEventStageRepository
{
    public EventStageRepository(ContestDbContext context) : base(context)
    {
    }

    public override async Task<EventStage?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(e => e.Event)
            .Include(e => e.PreviousStage)
            .Include(e => e.EventStageCriterias)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }
}

