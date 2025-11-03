using ContestService.Application.DTOs.EventStageCriteria;

namespace ContestService.Application.Services;

public interface IEventStageCriteriaService
{
    Task<EventStageCriteriaDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<EventStageCriteriaDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<EventStageCriteriaDto>> GetByEventStageIdAsync(int eventStageId, CancellationToken cancellationToken = default);
    Task<EventStageCriteriaDto> CreateAsync(CreateEventStageCriteriaRequest request, CancellationToken cancellationToken = default);
    Task<EventStageCriteriaDto> UpdateAsync(UpdateEventStageCriteriaRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}

