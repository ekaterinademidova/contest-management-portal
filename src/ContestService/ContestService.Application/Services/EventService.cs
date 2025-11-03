using ContestService.Application.DTOs.Event;
using ContestService.Domain.Entities;
using ContestService.Domain.Interfaces;

namespace ContestService.Application.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _repository;

    public EventService(IEventRepository repository)
    {
        _repository = repository;
    }

    public async Task<EventDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        if (entity == null)
            throw new KeyNotFoundException($"Event with ID {id} not found.");

        return MapToDto(entity);
    }

    public async Task<IEnumerable<EventDto>> GetAllAsync(EventFilterRequest? filter = null, CancellationToken cancellationToken = default)
    {
        IEnumerable<Event> entities;

        if (filter != null && (!string.IsNullOrWhiteSpace(filter.Name) || !string.IsNullOrWhiteSpace(filter.Description)))
        {
            entities = await _repository.GetAsync(e =>
                (string.IsNullOrWhiteSpace(filter.Name) || e.Name.Contains(filter.Name)) &&
                (string.IsNullOrWhiteSpace(filter.Description) || (e.Description != null && e.Description.Contains(filter.Description))),
                cancellationToken);
        }
        else
        {
            entities = await _repository.GetAllAsync(cancellationToken);
        }

        return entities.Select(MapToDto);
    }

    public async Task<EventDto> CreateAsync(CreateEventRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new Event
        {
            Name = request.Name,
            Description = request.Description
        };

        var created = await _repository.CreateAsync(entity, cancellationToken);
        return MapToDto(created);
    }

    public async Task<EventDto> UpdateAsync(UpdateEventRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
            throw new KeyNotFoundException($"Event with ID {request.Id} not found.");

        entity.Name = request.Name;
        entity.Description = request.Description;

        var updated = await _repository.UpdateAsync(entity, cancellationToken);
        return MapToDto(updated);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var exists = await _repository.ExistsAsync(id, cancellationToken);
        if (!exists)
            throw new KeyNotFoundException($"Event with ID {id} not found.");

        await _repository.DeleteAsync(id, cancellationToken);
    }

    private static EventDto MapToDto(Event entity)
    {
        return new EventDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description
        };
    }
}

