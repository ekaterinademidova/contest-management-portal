using ContestService.Application.DTOs.EventStageCriteria;
using ContestService.Domain.Entities;
using ContestService.Domain.Interfaces;

namespace ContestService.Application.Services;

public class EventStageCriteriaService : IEventStageCriteriaService
{
    private readonly IEventStageCriteriaRepository _repository;

    public EventStageCriteriaService(IEventStageCriteriaRepository repository)
    {
        _repository = repository;
    }

    public async Task<EventStageCriteriaDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        if (entity == null)
            throw new KeyNotFoundException($"EventStageCriteria with ID {id} not found.");

        return MapToDto(entity);
    }

    public async Task<IEnumerable<EventStageCriteriaDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return entities.Select(MapToDto);
    }

    public async Task<IEnumerable<EventStageCriteriaDto>> GetByEventStageIdAsync(int eventStageId, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.GetAsync(e => e.EventStageId == eventStageId, cancellationToken);
        return entities.Select(MapToDto);
    }

    public async Task<EventStageCriteriaDto> CreateAsync(CreateEventStageCriteriaRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new EventStageCriteria
        {
            EventStageId = request.EventStageId,
            Name = request.Name,
            IsActiveUntil = request.IsActiveUntil,
            IsRequired = request.IsRequired,
            MinimumThresholdScore = request.MinimumThresholdScore,
            Description = request.Description
        };

        var created = await _repository.CreateAsync(entity, cancellationToken);
        return MapToDto(created);
    }

    public async Task<EventStageCriteriaDto> UpdateAsync(UpdateEventStageCriteriaRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
            throw new KeyNotFoundException($"EventStageCriteria with ID {request.Id} not found.");

        entity.EventStageId = request.EventStageId;
        entity.Name = request.Name;
        entity.IsActiveUntil = request.IsActiveUntil;
        entity.IsRequired = request.IsRequired;
        entity.MinimumThresholdScore = request.MinimumThresholdScore;
        entity.Description = request.Description;

        var updated = await _repository.UpdateAsync(entity, cancellationToken);
        return MapToDto(updated);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var exists = await _repository.ExistsAsync(id, cancellationToken);
        if (!exists)
            throw new KeyNotFoundException($"EventStageCriteria with ID {id} not found.");

        await _repository.DeleteAsync(id, cancellationToken);
    }

    private static EventStageCriteriaDto MapToDto(EventStageCriteria entity)
    {
        return new EventStageCriteriaDto
        {
            Id = entity.Id,
            EventStageId = entity.EventStageId,
            Name = entity.Name,
            IsActiveUntil = entity.IsActiveUntil,
            IsRequired = entity.IsRequired,
            MinimumThresholdScore = entity.MinimumThresholdScore,
            Description = entity.Description
        };
    }
}

