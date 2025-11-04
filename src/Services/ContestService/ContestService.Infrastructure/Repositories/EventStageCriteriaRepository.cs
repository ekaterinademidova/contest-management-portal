using ContestService.Domain.Entities;
using ContestService.Domain.Interfaces;
using ContestService.Infrastructure.Data;

namespace ContestService.Infrastructure.Repositories;

public class EventStageCriteriaRepository : BaseRepository<EventStageCriteria>, IEventStageCriteriaRepository
{
    public EventStageCriteriaRepository(ContestDbContext context) : base(context)
    {
    }
}

