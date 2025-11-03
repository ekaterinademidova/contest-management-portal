using ContestService.Application.DTOs.EventStage;
using ContestService.Application.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ContestService.API.Controllers;

/// <summary>
/// Controller for managing Event Stages
/// </summary>
[ApiController]
[Route("api/[controller]")]
[SwaggerTag("Operations for managing event stages within contests")]
public class EventStagesController : ControllerBase
{
    private readonly IEventStageService _eventStageService;
    private readonly IValidator<CreateEventStageRequest> _createValidator;
    private readonly IValidator<UpdateEventStageRequest> _updateValidator;

    public EventStagesController(
        IEventStageService eventStageService,
        IValidator<CreateEventStageRequest> createValidator,
        IValidator<UpdateEventStageRequest> updateValidator)
    {
        _eventStageService = eventStageService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    /// <summary>
    /// Get an event stage by ID
    /// </summary>
    /// <param name="id">The unique identifier of the event stage</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The event stage details</returns>
    /// <response code="200">Returns the event stage successfully</response>
    /// <response code="404">Event stage with the specified ID was not found</response>
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get event stage by ID", Description = "Retrieves a specific event stage by its unique identifier.")]
    [SwaggerResponse(200, "Event stage retrieved successfully", typeof(EventStageDto))]
    [SwaggerResponse(404, "Event stage not found")]
    [ProducesResponseType(typeof(EventStageDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EventStageDto>> GetById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _eventStageService.GetByIdAsync(id, cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Get all event stages with optional filtering
    /// </summary>
    /// <param name="filter">Optional filter parameters (eventId, previousStageId, name)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of event stages</returns>
    /// <response code="200">Returns the list of event stages successfully</response>
    [HttpGet]
    [SwaggerOperation(Summary = "Get all event stages", Description = "Retrieves all event stages. Supports optional filtering by eventId, previousStageId, and name.")]
    [SwaggerResponse(200, "Event stages retrieved successfully", typeof(IEnumerable<EventStageDto>))]
    [ProducesResponseType(typeof(IEnumerable<EventStageDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<EventStageDto>>> GetAll([FromQuery] EventStageFilterRequest? filter, CancellationToken cancellationToken)
    {
        var result = await _eventStageService.GetAllAsync(filter, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Create a new event stage
    /// </summary>
    /// <param name="request">The event stage creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created event stage</returns>
    /// <response code="201">Event stage created successfully</response>
    /// <response code="400">Invalid request data or validation errors</response>
    [HttpPost]
    [SwaggerOperation(Summary = "Create a new event stage", Description = "Creates a new event stage with the provided information.")]
    [SwaggerResponse(201, "Event stage created successfully", typeof(EventStageDto))]
    [SwaggerResponse(400, "Bad request - validation errors")]
    [ProducesResponseType(typeof(EventStageDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EventStageDto>> Create([FromBody] CreateEventStageRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var result = await _eventStageService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Update an existing event stage
    /// </summary>
    /// <param name="id">The unique identifier of the event stage to update</param>
    /// <param name="request">The event stage update request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated event stage</returns>
    /// <response code="200">Event stage updated successfully</response>
    /// <response code="400">Invalid request data or ID mismatch</response>
    /// <response code="404">Event stage with the specified ID was not found</response>
    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update an event stage", Description = "Updates an existing event stage with the provided information.")]
    [SwaggerResponse(200, "Event stage updated successfully", typeof(EventStageDto))]
    [SwaggerResponse(400, "Bad request - validation errors or ID mismatch")]
    [SwaggerResponse(404, "Event stage not found")]
    [ProducesResponseType(typeof(EventStageDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EventStageDto>> Update(int id, [FromBody] UpdateEventStageRequest request, CancellationToken cancellationToken)
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
            var result = await _eventStageService.UpdateAsync(request, cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Delete an event stage
    /// </summary>
    /// <param name="id">The unique identifier of the event stage to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>No content</returns>
    /// <response code="204">Event stage deleted successfully</response>
    /// <response code="404">Event stage with the specified ID was not found</response>
    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete an event stage", Description = "Deletes an event stage by its unique identifier.")]
    [SwaggerResponse(204, "Event stage deleted successfully")]
    [SwaggerResponse(404, "Event stage not found")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _eventStageService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
