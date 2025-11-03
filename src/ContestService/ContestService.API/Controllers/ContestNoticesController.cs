using ContestService.Application.DTOs.ContestNotice;
using ContestService.Application.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ContestService.API.Controllers;

/// <summary>
/// Controller for managing Contest Notices
/// </summary>
[ApiController]
[Route("api/[controller]")]
[SwaggerTag("Operations for managing contest notices and announcements")]
public class ContestNoticesController : ControllerBase
{
    private readonly IContestNoticeService _service;
    private readonly IValidator<CreateContestNoticeRequest> _createValidator;
    private readonly IValidator<UpdateContestNoticeRequest> _updateValidator;

    public ContestNoticesController(
        IContestNoticeService service,
        IValidator<CreateContestNoticeRequest> createValidator,
        IValidator<UpdateContestNoticeRequest> updateValidator)
    {
        _service = service;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    /// <summary>
    /// Get a contest notice by ID
    /// </summary>
    /// <param name="id">The unique identifier of the contest notice</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The contest notice details</returns>
    /// <response code="200">Returns the contest notice successfully</response>
    /// <response code="404">Contest notice with the specified ID was not found</response>
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get contest notice by ID", Description = "Retrieves a specific contest notice by its unique identifier.")]
    [SwaggerResponse(200, "Contest notice retrieved successfully", typeof(ContestNoticeDto))]
    [SwaggerResponse(404, "Contest notice not found")]
    [ProducesResponseType(typeof(ContestNoticeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContestNoticeDto>> GetById(int id, CancellationToken cancellationToken)
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
    /// Get all contest notices with optional filtering
    /// </summary>
    /// <param name="filter">Optional filter parameters (organizerId, eventId, competitionType)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of contest notices</returns>
    /// <response code="200">Returns the list of contest notices successfully</response>
    [HttpGet]
    [SwaggerOperation(Summary = "Get all contest notices", Description = "Retrieves all contest notices. Supports optional filtering by organizerId, eventId, and competitionType.")]
    [SwaggerResponse(200, "Contest notices retrieved successfully", typeof(IEnumerable<ContestNoticeDto>))]
    [ProducesResponseType(typeof(IEnumerable<ContestNoticeDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ContestNoticeDto>>> GetAll([FromQuery] ContestNoticeFilterRequest? filter, CancellationToken cancellationToken)
    {
        var result = await _service.GetAllAsync(filter, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Create a new contest notice
    /// </summary>
    /// <param name="request">The contest notice creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created contest notice</returns>
    /// <response code="201">Contest notice created successfully</response>
    /// <response code="400">Invalid request data or validation errors</response>
    [HttpPost]
    [SwaggerOperation(Summary = "Create a new contest notice", Description = "Creates a new contest notice with the provided information.")]
    [SwaggerResponse(201, "Contest notice created successfully", typeof(ContestNoticeDto))]
    [SwaggerResponse(400, "Bad request - validation errors")]
    [ProducesResponseType(typeof(ContestNoticeDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ContestNoticeDto>> Create([FromBody] CreateContestNoticeRequest request, CancellationToken cancellationToken)
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
    /// Update an existing contest notice
    /// </summary>
    /// <param name="id">The unique identifier of the contest notice to update</param>
    /// <param name="request">The contest notice update request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated contest notice</returns>
    /// <response code="200">Contest notice updated successfully</response>
    /// <response code="400">Invalid request data or ID mismatch</response>
    /// <response code="404">Contest notice with the specified ID was not found</response>
    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update a contest notice", Description = "Updates an existing contest notice with the provided information.")]
    [SwaggerResponse(200, "Contest notice updated successfully", typeof(ContestNoticeDto))]
    [SwaggerResponse(400, "Bad request - validation errors or ID mismatch")]
    [SwaggerResponse(404, "Contest notice not found")]
    [ProducesResponseType(typeof(ContestNoticeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContestNoticeDto>> Update(int id, [FromBody] UpdateContestNoticeRequest request, CancellationToken cancellationToken)
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
    /// Delete a contest notice
    /// </summary>
    /// <param name="id">The unique identifier of the contest notice to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>No content</returns>
    /// <response code="204">Contest notice deleted successfully</response>
    /// <response code="404">Contest notice with the specified ID was not found</response>
    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete a contest notice", Description = "Deletes a contest notice by its unique identifier.")]
    [SwaggerResponse(204, "Contest notice deleted successfully")]
    [SwaggerResponse(404, "Contest notice not found")]
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
