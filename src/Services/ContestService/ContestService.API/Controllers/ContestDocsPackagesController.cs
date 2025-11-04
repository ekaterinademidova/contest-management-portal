using ContestService.Application.DTOs.ContestDocsPackage;
using ContestService.Application.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ContestService.API.Controllers;

/// <summary>
/// Controller for managing Contest Documentation Packages
/// </summary>
[ApiController]
[Route("api/[controller]")]
[SwaggerTag("Operations for managing documentation packages linking contest notices to attachments")]
public class ContestDocsPackagesController : ControllerBase
{
    private readonly IContestDocsPackageService _service;
    private readonly IValidator<CreateContestDocsPackageRequest> _createValidator;
    private readonly IValidator<UpdateContestDocsPackageRequest> _updateValidator;

    public ContestDocsPackagesController(
        IContestDocsPackageService service,
        IValidator<CreateContestDocsPackageRequest> createValidator,
        IValidator<UpdateContestDocsPackageRequest> updateValidator)
    {
        _service = service;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    /// <summary>
    /// Get a contest docs package by ID
    /// </summary>
    /// <param name="id">The unique identifier of the contest docs package</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The contest docs package details</returns>
    /// <response code="200">Returns the contest docs package successfully</response>
    /// <response code="404">Contest docs package with the specified ID was not found</response>
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get contest docs package by ID", Description = "Retrieves a specific contest docs package by its unique identifier.")]
    [SwaggerResponse(200, "Contest docs package retrieved successfully", typeof(ContestDocsPackageDto))]
    [SwaggerResponse(404, "Contest docs package not found")]
    [ProducesResponseType(typeof(ContestDocsPackageDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContestDocsPackageDto>> GetById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _service.GetByIdAsync(id, cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Get all contest docs packages
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of contest docs packages</returns>
    /// <response code="200">Returns the list of contest docs packages successfully</response>
    [HttpGet]
    [SwaggerOperation(Summary = "Get all contest docs packages", Description = "Retrieves all contest docs packages in the system.")]
    [SwaggerResponse(200, "Contest docs packages retrieved successfully", typeof(IEnumerable<ContestDocsPackageDto>))]
    [ProducesResponseType(typeof(IEnumerable<ContestDocsPackageDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ContestDocsPackageDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _service.GetAllAsync(cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get contest docs packages by contest notice ID
    /// </summary>
    /// <param name="contestNoticeId">The unique identifier of the contest notice</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of documentation packages for the specified contest notice</returns>
    /// <response code="200">Returns the list of packages successfully</response>
    [HttpGet("contest-notice/{contestNoticeId}")]
    [SwaggerOperation(Summary = "Get packages by contest notice ID", Description = "Retrieves all documentation packages associated with a specific contest notice.")]
    [SwaggerResponse(200, "Packages retrieved successfully", typeof(IEnumerable<ContestDocsPackageDto>))]
    [ProducesResponseType(typeof(IEnumerable<ContestDocsPackageDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ContestDocsPackageDto>>> GetByContestNoticeId(int contestNoticeId, CancellationToken cancellationToken)
    {
        var result = await _service.GetByContestNoticeIdAsync(contestNoticeId, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Create a new contest docs package
    /// </summary>
    /// <param name="request">The contest docs package creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created contest docs package</returns>
    /// <response code="201">Contest docs package created successfully</response>
    /// <response code="400">Invalid request data or validation errors</response>
    [HttpPost]
    [SwaggerOperation(Summary = "Create a new contest docs package", Description = "Creates a new contest docs package linking a contest notice to an attachment.")]
    [SwaggerResponse(201, "Contest docs package created successfully", typeof(ContestDocsPackageDto))]
    [SwaggerResponse(400, "Bad request - validation errors")]
    [ProducesResponseType(typeof(ContestDocsPackageDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ContestDocsPackageDto>> Create([FromBody] CreateContestDocsPackageRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var result = await _service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Update an existing contest docs package
    /// </summary>
    /// <param name="id">The unique identifier of the contest docs package to update</param>
    /// <param name="request">The contest docs package update request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated contest docs package</returns>
    /// <response code="200">Contest docs package updated successfully</response>
    /// <response code="400">Invalid request data or ID mismatch</response>
    /// <response code="404">Contest docs package with the specified ID was not found</response>
    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update a contest docs package", Description = "Updates an existing contest docs package with the provided information.")]
    [SwaggerResponse(200, "Contest docs package updated successfully", typeof(ContestDocsPackageDto))]
    [SwaggerResponse(400, "Bad request - validation errors or ID mismatch")]
    [SwaggerResponse(404, "Contest docs package not found")]
    [ProducesResponseType(typeof(ContestDocsPackageDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContestDocsPackageDto>> Update(int id, [FromBody] UpdateContestDocsPackageRequest request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
        {
            return BadRequest("Id in URL does not match Id in request body.");
        }

        var validationResult = await _updateValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        try
        {
            var result = await _service.UpdateAsync(request, cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Delete a contest docs package
    /// </summary>
    /// <param name="id">The unique identifier of the contest docs package to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>No content</returns>
    /// <response code="204">Contest docs package deleted successfully</response>
    /// <response code="404">Contest docs package with the specified ID was not found</response>
    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete a contest docs package", Description = "Deletes a contest docs package by its unique identifier.")]
    [SwaggerResponse(204, "Contest docs package deleted successfully")]
    [SwaggerResponse(404, "Contest docs package not found")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _service.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
