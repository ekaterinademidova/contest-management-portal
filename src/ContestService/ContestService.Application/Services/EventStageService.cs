using ContestService.Application.DTOs.EventStage;
using ContestService.Domain.Entities;
using ContestService.Domain.Interfaces;

namespace ContestService.Application.Services;

public class EventStageService : IEventStageService
{
    private readonly IEventStageRepository _repository;

    public EventStageService(IEventStageRepository repository)
    {
        _repository = repository;
    }

    public async Task<EventStageDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        if (entity == null)
            throw new KeyNotFoundException($"EventStage with ID {id} not found.");

        return MapToDto(entity);
    }

    public async Task<IEnumerable<EventStageDto>> GetAllAsync(EventStageFilterRequest? filter = null, CancellationToken cancellationToken = default)
    {
        IEnumerable<EventStage> entities;

        if (filter != null)
        {
            entities = await _repository.GetAsync(e =>
                (!filter.EventId.HasValue || e.EventId == filter.EventId.Value) &&
                (!filter.PreviousStageId.HasValue || e.PreviousStageId == filter.PreviousStageId.Value) &&
                (string.IsNullOrWhiteSpace(filter.Name) || e.Name.Contains(filter.Name)),
                cancellationToken);
        }
        else
        {
            entities = await _repository.GetAllAsync(cancellationToken);
        }

        return entities.Select(MapToDto);
    }

    public async Task<EventStageDto> CreateAsync(CreateEventStageRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new EventStage
        {
            PreviousStageId = request.PreviousStageId,
            EventId = request.EventId,
            DateStart = request.DateStart,
            DateEnd = request.DateEnd,
            Name = request.Name,
            Description = request.Description
        };

        var created = await _repository.CreateAsync(entity, cancellationToken);
        return MapToDto(created);
    }

    public async Task<EventStageDto> UpdateAsync(UpdateEventStageRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
            throw new KeyNotFoundException($"EventStage with ID {request.Id} not found.");

        entity.PreviousStageId = request.PreviousStageId;
        entity.EventId = request.EventId;
        entity.DateStart = request.DateStart;
        entity.DateEnd = request.DateEnd;
        entity.Name = request.Name;
        entity.Description = request.Description;

        var updated = await _repository.UpdateAsync(entity, cancellationToken);
        return MapToDto(updated);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var exists = await _repository.ExistsAsync(id, cancellationToken);
        if (!exists)
            throw new KeyNotFoundException($"EventStage with ID {id} not found.");

        await _repository.DeleteAsync(id, cancellationToken);
    }

    private static EventStageDto MapToDto(EventStage entity)
    {
        return new EventStageDto
        {
            Id = entity.Id,
            PreviousStageId = entity.PreviousStageId,
            EventId = entity.EventId,
            DateStart = entity.DateStart,
            DateEnd = entity.DateEnd,
            Name = entity.Name,
            Description = entity.Description
        };
    }
}

