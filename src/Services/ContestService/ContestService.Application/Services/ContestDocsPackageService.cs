using ContestService.Application.DTOs.ContestDocsPackage;
using ContestService.Domain.Entities;
using ContestService.Domain.Interfaces;

namespace ContestService.Application.Services;

public class ContestDocsPackageService : IContestDocsPackageService
{
    private readonly IContestDocsPackageRepository _repository;

    public ContestDocsPackageService(IContestDocsPackageRepository repository)
    {
        _repository = repository;
    }

    public async Task<ContestDocsPackageDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        if (entity == null)
            throw new KeyNotFoundException($"ContestDocsPackage with ID {id} not found.");

        return MapToDto(entity);
    }

    public async Task<IEnumerable<ContestDocsPackageDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return entities.Select(MapToDto);
    }

    public async Task<IEnumerable<ContestDocsPackageDto>> GetByContestNoticeIdAsync(int contestNoticeId, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.GetAsync(c => c.ContestNoticeId == contestNoticeId, cancellationToken);
        return entities.Select(MapToDto);
    }

    public async Task<ContestDocsPackageDto> CreateAsync(CreateContestDocsPackageRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new ContestDocsPackage
        {
            ContestNoticeId = request.ContestNoticeId,
            AttachmentId = request.AttachmentId,
            IsRequired = request.IsRequired,
            Comment = request.Comment
        };

        var created = await _repository.CreateAsync(entity, cancellationToken);
        return MapToDto(created);
    }

    public async Task<ContestDocsPackageDto> UpdateAsync(UpdateContestDocsPackageRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
            throw new KeyNotFoundException($"ContestDocsPackage with ID {request.Id} not found.");

        entity.ContestNoticeId = request.ContestNoticeId;
        entity.AttachmentId = request.AttachmentId;
        entity.IsRequired = request.IsRequired;
        entity.Comment = request.Comment;

        var updated = await _repository.UpdateAsync(entity, cancellationToken);
        return MapToDto(updated);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var exists = await _repository.ExistsAsync(id, cancellationToken);
        if (!exists)
            throw new KeyNotFoundException($"ContestDocsPackage with ID {id} not found.");

        await _repository.DeleteAsync(id, cancellationToken);
    }

    private static ContestDocsPackageDto MapToDto(ContestDocsPackage entity)
    {
        return new ContestDocsPackageDto
        {
            Id = entity.Id,
            ContestNoticeId = entity.ContestNoticeId,
            AttachmentId = entity.AttachmentId,
            IsRequired = entity.IsRequired,
            Comment = entity.Comment
        };
    }
}

