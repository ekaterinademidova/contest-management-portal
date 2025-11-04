using ContestService.Application.DTOs.ContestNotice;
using ContestService.Domain.Entities;
using ContestService.Domain.Interfaces;

namespace ContestService.Application.Services;

public class ContestNoticeService : IContestNoticeService
{
    private readonly IContestNoticeRepository _repository;

    public ContestNoticeService(IContestNoticeRepository repository)
    {
        _repository = repository;
    }

    public async Task<ContestNoticeDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        if (entity == null)
            throw new KeyNotFoundException($"ContestNotice with ID {id} not found.");

        return MapToDto(entity);
    }

    public async Task<IEnumerable<ContestNoticeDto>> GetAllAsync(ContestNoticeFilterRequest? filter = null, CancellationToken cancellationToken = default)
    {
        IEnumerable<ContestNotice> entities;

        if (filter != null)
        {
            entities = await _repository.GetAsync(c =>
                (!filter.OrganizerId.HasValue || c.OrganizerId == filter.OrganizerId.Value) &&
                (!filter.EventId.HasValue || c.EventId == filter.EventId.Value) &&
                (string.IsNullOrWhiteSpace(filter.CompetitionType) || c.CompetitionType == filter.CompetitionType),
                cancellationToken);
        }
        else
        {
            entities = await _repository.GetAllAsync(cancellationToken);
        }

        return entities.Select(MapToDto);
    }

    public async Task<ContestNoticeDto> CreateAsync(CreateContestNoticeRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new ContestNotice
        {
            OrganizerId = request.OrganizerId,
            EventId = request.EventId,
            Goal = request.Goal,
            CompetitionType = request.CompetitionType,
            Description = request.Description
        };

        var created = await _repository.CreateAsync(entity, cancellationToken);
        return MapToDto(created);
    }

    public async Task<ContestNoticeDto> UpdateAsync(UpdateContestNoticeRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
            throw new KeyNotFoundException($"ContestNotice with ID {request.Id} not found.");

        entity.OrganizerId = request.OrganizerId;
        entity.EventId = request.EventId;
        entity.Goal = request.Goal;
        entity.CompetitionType = request.CompetitionType;
        entity.Description = request.Description;

        var updated = await _repository.UpdateAsync(entity, cancellationToken);
        return MapToDto(updated);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var exists = await _repository.ExistsAsync(id, cancellationToken);
        if (!exists)
            throw new KeyNotFoundException($"ContestNotice with ID {id} not found.");

        await _repository.DeleteAsync(id, cancellationToken);
    }

    private static ContestNoticeDto MapToDto(ContestNotice entity)
    {
        return new ContestNoticeDto
        {
            Id = entity.Id,
            OrganizerId = entity.OrganizerId,
            EventId = entity.EventId,
            Goal = entity.Goal,
            CompetitionType = entity.CompetitionType,
            Description = entity.Description
        };
    }
}

