using ContestService.Application.DTOs.EventStageCriteria;
using ContestService.Application.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ContestService.API.Controllers;

/// <summary>
/// Controller for managing Event Stage Criteria
/// </summary>
[ApiController]
[Route("api/[controller]")]
[SwaggerTag("Operations for managing evaluation criteria for event stages")]
public class EventStageCriteriasController : ControllerBase
{
    private readonly IEventStageCriteriaService _service;
    private readonly IValidator<CreateEventStageCriteriaRequest> _createValidator;
    private readonly IValidator<UpdateEventStageCriteriaRequest> _updateValidator;

    public EventStageCriteriasController(
        IEventStageCriteriaService service,
        IValidator<CreateEventStageCriteriaRequest> createValidator,
        IValidator<UpdateEventStageCriteriaRequest> updateValidator)
    {
        _service = service;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    /// <summary>
    /// Get an event stage criteria by ID
    /// </summary>
    /// <param name="id">The unique identifier of the event stage criteria</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The event stage criteria details</returns>
    /// <response code="200">Returns the event stage criteria successfully</response>
    /// <response code="404">Event stage criteria with the specified ID was not found</response>
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get event stage criteria by ID", Description = "Retrieves a specific event stage criteria by its unique identifier.")]
    [SwaggerResponse(200, "Event stage criteria retrieved successfully", typeof(EventStageCriteriaDto))]
    [SwaggerResponse(404, "Event stage criteria not found")]
    [ProducesResponseType(typeof(EventStageCriteriaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EventStageCriteriaDto>> GetById(int id, CancellationToken cancellationToken)
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
    /// Get all event stage criteria
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of event stage criteria</returns>
    /// <response code="200">Returns the list of event stage criteria successfully</response>
    [HttpGet]
    [SwaggerOperation(Summary = "Get all event stage criteria", Description = "Retrieves all event stage criteria.")]
    [SwaggerResponse(200, "Event stage criteria retrieved successfully", typeof(IEnumerable<EventStageCriteriaDto>))]
    [ProducesResponseType(typeof(IEnumerable<EventStageCriteriaDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<EventStageCriteriaDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _service.GetAllAsync(cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get event stage criteria by event stage ID
    /// </summary>
    /// <param name="eventStageId">The unique identifier of the event stage</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of criteria for the specified event stage</returns>
    /// <response code="200">Returns the list of criteria successfully</response>
    [HttpGet("event-stage/{eventStageId}")]
    [SwaggerOperation(Summary = "Get criteria by event stage ID", Description = "Retrieves all criteria associated with a specific event stage.")]
    [SwaggerResponse(200, "Criteria retrieved successfully", typeof(IEnumerable<EventStageCriteriaDto>))]
    [ProducesResponseType(typeof(IEnumerable<EventStageCriteriaDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<EventStageCriteriaDto>>> GetByEventStageId(int eventStageId, CancellationToken cancellationToken)
    {
        var result = await _service.GetByEventStageIdAsync(eventStageId, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Create a new event stage criteria
    /// </summary>
    /// <param name="request">The event stage criteria creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created event stage criteria</returns>
    /// <response code="201">Event stage criteria created successfully</response>
    /// <response code="400">Invalid request data or validation errors</response>
    [HttpPost]
    [SwaggerOperation(Summary = "Create a new event stage criteria", Description = "Creates a new event stage criteria with the provided information.")]
    [SwaggerResponse(201, "Event stage criteria created successfully", typeof(EventStageCriteriaDto))]
    [SwaggerResponse(400, "Bad request - validation errors")]
    [ProducesResponseType(typeof(EventStageCriteriaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EventStageCriteriaDto>> Create([FromBody] CreateEventStageCriteriaRequest request, CancellationToken cancellationToken)
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
    /// Update an existing event stage criteria
    /// </summary>
    /// <param name="id">The unique identifier of the event stage criteria to update</param>
    /// <param name="request">The event stage criteria update request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated event stage criteria</returns>
    /// <response code="200">Event stage criteria updated successfully</response>
    /// <response code="400">Invalid request data or ID mismatch</response>
    /// <response code="404">Event stage criteria with the specified ID was not found</response>
    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update an event stage criteria", Description = "Updates an existing event stage criteria with the provided information.")]
    [SwaggerResponse(200, "Event stage criteria updated successfully", typeof(EventStageCriteriaDto))]
    [SwaggerResponse(400, "Bad request - validation errors or ID mismatch")]
    [SwaggerResponse(404, "Event stage criteria not found")]
    [ProducesResponseType(typeof(EventStageCriteriaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EventStageCriteriaDto>> Update(int id, [FromBody] UpdateEventStageCriteriaRequest request, CancellationToken cancellationToken)
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
    /// Delete an event stage criteria
    /// </summary>
    /// <param name="id">The unique identifier of the event stage criteria to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>No content</returns>
    /// <response code="204">Event stage criteria deleted successfully</response>
    /// <response code="404">Event stage criteria with the specified ID was not found</response>
    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete an event stage criteria", Description = "Deletes an event stage criteria by its unique identifier.")]
    [SwaggerResponse(204, "Event stage criteria deleted successfully")]
    [SwaggerResponse(404, "Event stage criteria not found")]
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
