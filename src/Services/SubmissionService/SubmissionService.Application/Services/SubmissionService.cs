using SubmissionService.Application.DTOs.Appeal;
using SubmissionService.Application.DTOs.Submission;
using SubmissionService.Domain.Entities;
using SubmissionService.Domain.Interfaces;

namespace SubmissionService.Application.Services;

public class SubmissionService : ISubmissionService
{
    private readonly ISubmissionRepository _submissionRepository;
    private readonly IAppealRepository _appealRepository;

    public SubmissionService(
        ISubmissionRepository submissionRepository,
        IAppealRepository appealRepository)
    {
        _submissionRepository = submissionRepository;
        _appealRepository = appealRepository;
    }

    #region Submission Methods

    public async Task<SubmissionDto> GetSubmissionByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _submissionRepository.GetByIdAsync(id, cancellationToken);
        if (entity == null)
            throw new KeyNotFoundException($"Submission with ID {id} not found.");

        return MapToSubmissionDto(entity);
    }

    public async Task<IEnumerable<SubmissionDto>> GetAllSubmissionsAsync(SubmissionFilterRequest? filter = null, CancellationToken cancellationToken = default)
    {
        IEnumerable<Submission> entities;

        if (filter != null)
        {
            entities = await _submissionRepository.GetAsync(s =>
                (!filter.ContestNoticeId.HasValue || s.ContestNoticeId == filter.ContestNoticeId.Value) &&
                (!filter.ParticipantId.HasValue || s.ParticipantId == filter.ParticipantId.Value) &&
                (string.IsNullOrWhiteSpace(filter.SubmissionState) || s.SubmissionState == filter.SubmissionState),
                cancellationToken);
        }
        else
        {
            entities = await _submissionRepository.GetAllAsync(cancellationToken);
        }

        return entities.Select(MapToSubmissionDto);
    }

    public async Task<SubmissionDto> CreateSubmissionAsync(CreateSubmissionRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new Submission
        {
            ContestNoticeId = request.ContestNoticeId,
            ParticipantId = request.ParticipantId,
            DateTime = DateTime.UtcNow,
            CoverLetter = request.CoverLetter,
            Comment = request.Comment,
            DocsPackageIsValid = request.DocsPackageIsValid,
            SubmissionState = request.SubmissionState ?? "Pending"
        };

        var created = await _submissionRepository.CreateAsync(entity, cancellationToken);
        return MapToSubmissionDto(created);
    }

    public async Task<SubmissionDto> UpdateSubmissionAsync(UpdateSubmissionRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _submissionRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
            throw new KeyNotFoundException($"Submission with ID {request.Id} not found.");

        entity.ContestNoticeId = request.ContestNoticeId;
        entity.ParticipantId = request.ParticipantId;
        entity.CoverLetter = request.CoverLetter;
        entity.Comment = request.Comment;
        entity.DocsPackageIsValid = request.DocsPackageIsValid;
        entity.SubmissionState = request.SubmissionState;

        var updated = await _submissionRepository.UpdateAsync(entity, cancellationToken);
        return MapToSubmissionDto(updated);
    }

    public async Task DeleteSubmissionAsync(int id, CancellationToken cancellationToken = default)
    {
        var exists = await _submissionRepository.ExistsAsync(id, cancellationToken);
        if (!exists)
            throw new KeyNotFoundException($"Submission with ID {id} not found.");

        // Check if there's an associated appeal
        var appeal = await _appealRepository.GetBySubmissionIdAsync(id, cancellationToken);
        if (appeal != null)
            throw new InvalidOperationException($"Cannot delete Submission with ID {id} because it has an associated Appeal. Delete the Appeal first.");

        await _submissionRepository.DeleteAsync(id, cancellationToken);
    }

    #endregion

    #region Appeal Methods

    public async Task<AppealDto> GetAppealByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _appealRepository.GetByIdAsync(id, cancellationToken);
        if (entity == null)
            throw new KeyNotFoundException($"Appeal with ID {id} not found.");

        return MapToAppealDto(entity);
    }

    public async Task<AppealDto?> GetAppealBySubmissionIdAsync(int submissionId, CancellationToken cancellationToken = default)
    {
        var entity = await _appealRepository.GetBySubmissionIdAsync(submissionId, cancellationToken);
        return entity == null ? null : MapToAppealDto(entity);
    }

    public async Task<IEnumerable<AppealDto>> GetAllAppealsAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _appealRepository.GetAllAsync(cancellationToken);
        return entities.Select(MapToAppealDto);
    }

    public async Task<AppealDto> CreateAppealAsync(CreateAppealRequest request, CancellationToken cancellationToken = default)
    {
        // Validate that Submission exists
        var submissionExists = await _submissionRepository.ExistsAsync(request.SubmissionId, cancellationToken);
        if (!submissionExists)
            throw new ArgumentException($"Submission with ID {request.SubmissionId} does not exist.");

        // Validate that there's no existing appeal for this submission (1:1 relationship)
        var existingAppeal = await _appealRepository.GetBySubmissionIdAsync(request.SubmissionId, cancellationToken);
        if (existingAppeal != null)
            throw new InvalidOperationException($"An Appeal already exists for Submission with ID {request.SubmissionId}.");

        var entity = new Appeal
        {
            SubmissionId = request.SubmissionId,
            DateTime = DateTime.UtcNow,
            CoverLetter = request.CoverLetter,
            AppealState = request.AppealState ?? "Pending",
            Comment = request.Comment
        };

        var created = await _appealRepository.CreateAsync(entity, cancellationToken);
        return MapToAppealDto(created);
    }

    public async Task<AppealDto> UpdateAppealAsync(UpdateAppealRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _appealRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
            throw new KeyNotFoundException($"Appeal with ID {request.Id} not found.");

        // Validate that Submission exists
        var submissionExists = await _submissionRepository.ExistsAsync(request.SubmissionId, cancellationToken);
        if (!submissionExists)
            throw new ArgumentException($"Submission with ID {request.SubmissionId} does not exist.");

        entity.SubmissionId = request.SubmissionId;
        entity.CoverLetter = request.CoverLetter;
        entity.AppealState = request.AppealState;
        entity.Comment = request.Comment;

        var updated = await _appealRepository.UpdateAsync(entity, cancellationToken);
        return MapToAppealDto(updated);
    }

    public async Task DeleteAppealAsync(int id, CancellationToken cancellationToken = default)
    {
        var exists = await _appealRepository.ExistsAsync(id, cancellationToken);
        if (!exists)
            throw new KeyNotFoundException($"Appeal with ID {id} not found.");

        await _appealRepository.DeleteAsync(id, cancellationToken);
    }

    #endregion

    #region Mapping Methods

    private static SubmissionDto MapToSubmissionDto(Submission entity)
    {
        return new SubmissionDto
        {
            Id = entity.Id,
            ContestNoticeId = entity.ContestNoticeId,
            ParticipantId = entity.ParticipantId,
            DateTime = entity.DateTime,
            CoverLetter = entity.CoverLetter,
            Comment = entity.Comment,
            DocsPackageIsValid = entity.DocsPackageIsValid,
            SubmissionState = entity.SubmissionState
        };
    }

    private static AppealDto MapToAppealDto(Appeal entity)
    {
        return new AppealDto
        {
            Id = entity.Id,
            SubmissionId = entity.SubmissionId,
            DateTime = entity.DateTime,
            CoverLetter = entity.CoverLetter,
            AppealState = entity.AppealState,
            Comment = entity.Comment
        };
    }

    #endregion
}

