using ContestService.Application.DTOs.Attachment;
using ContestService.Domain.Entities;
using ContestService.Domain.Interfaces;

namespace ContestService.Application.Services;

public class AttachmentService : IAttachmentService
{
    private readonly IAttachmentRepository _repository;

    public AttachmentService(IAttachmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<AttachmentDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        if (entity == null)
            throw new KeyNotFoundException($"Attachment with ID {id} not found.");

        return MapToDto(entity);
    }

    public async Task<IEnumerable<AttachmentDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return entities.Select(MapToDto);
    }

    public async Task<AttachmentDto> CreateAsync(CreateAttachmentRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new Attachment
        {
            Name = request.Name
        };

        var created = await _repository.CreateAsync(entity, cancellationToken);
        return MapToDto(created);
    }

    public async Task<AttachmentDto> UpdateAsync(UpdateAttachmentRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
            throw new KeyNotFoundException($"Attachment with ID {request.Id} not found.");

        entity.Name = request.Name;

        var updated = await _repository.UpdateAsync(entity, cancellationToken);
        return MapToDto(updated);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var exists = await _repository.ExistsAsync(id, cancellationToken);
        if (!exists)
            throw new KeyNotFoundException($"Attachment with ID {id} not found.");

        await _repository.DeleteAsync(id, cancellationToken);
    }

    private static AttachmentDto MapToDto(Attachment entity)
    {
        return new AttachmentDto
        {
            Id = entity.Id,
            Name = entity.Name
        };
    }
}

