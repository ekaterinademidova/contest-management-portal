using ContestService.Application.DTOs.Event;

namespace ContestService.Application.Services;

public interface IEventService
{
    Task<EventDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<EventDto>> GetAllAsync(EventFilterRequest? filter = null, CancellationToken cancellationToken = default);
    Task<EventDto> CreateAsync(CreateEventRequest request, CancellationToken cancellationToken = default);
    Task<EventDto> UpdateAsync(UpdateEventRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}

