using ContestService.Application.DTOs.EventStage;

namespace ContestService.Application.Services;

public interface IEventStageService
{
    Task<EventStageDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<EventStageDto>> GetAllAsync(EventStageFilterRequest? filter = null, CancellationToken cancellationToken = default);
    Task<EventStageDto> CreateAsync(CreateEventStageRequest request, CancellationToken cancellationToken = default);
    Task<EventStageDto> UpdateAsync(UpdateEventStageRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}

