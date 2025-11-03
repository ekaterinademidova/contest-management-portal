using ContestService.Application.DTOs.Event;
using ContestService.Application.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ContestService.API.Controllers;

/// <summary>
/// Controller for managing Events
/// </summary>
[ApiController]
[Route("api/[controller]")]
[SwaggerTag("Operations for managing contest events")]
public class EventsController : ControllerBase
{
    private readonly IEventService _eventService;
    private readonly IValidator<CreateEventRequest> _createValidator;
    private readonly IValidator<UpdateEventRequest> _updateValidator;

    public EventsController(
        IEventService eventService,
        IValidator<CreateEventRequest> createValidator,
        IValidator<UpdateEventRequest> updateValidator)
    {
        _eventService = eventService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    /// <summary>
    /// Get an event by ID
    /// </summary>
    /// <param name="id">The unique identifier of the event</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The event details</returns>
    /// <response code="200">Returns the event successfully</response>
    /// <response code="404">Event with the specified ID was not found</response>
    /// <remarks>
    /// Sample request:
    /// 
    ///     GET /api/Events/1
    /// 
    /// Sample response:
    /// 
    ///     {
    ///         "id": 1,
    ///         "name": "Annual Programming Championship 2024",
    ///         "description": "A prestigious programming competition..."
    ///     }
    /// </remarks>
    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Get event by ID",
        Description = "Retrieves a specific event by its unique identifier."
    )]
    [SwaggerResponse(200, "Event retrieved successfully", typeof(EventDto))]
    [SwaggerResponse(404, "Event not found")]
    [ProducesResponseType(typeof(EventDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EventDto>> GetById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _eventService.GetByIdAsync(id, cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Get all events with optional filtering
    /// </summary>
    /// <param name="filter">Optional filter parameters (name, description)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of events</returns>
    /// <response code="200">Returns the list of events successfully</response>
    /// <remarks>
    /// Sample request:
    /// 
    ///     GET /api/Events?name=Programming
    /// 
    /// Sample response:
    /// 
    ///     [
    ///         {
    ///             "id": 1,
    ///             "name": "Annual Programming Championship 2024",
    ///             "description": "A prestigious programming competition..."
    ///         }
    ///     ]
    /// </remarks>
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all events",
        Description = "Retrieves all events. Supports optional filtering by name and description."
    )]
    [SwaggerResponse(200, "Events retrieved successfully", typeof(IEnumerable<EventDto>))]
    [ProducesResponseType(typeof(IEnumerable<EventDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<EventDto>>> GetAll(
        [FromQuery] EventFilterRequest? filter, 
        CancellationToken cancellationToken)
    {
        var result = await _eventService.GetAllAsync(filter, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Create a new event
    /// </summary>
    /// <param name="request">The event creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created event</returns>
    /// <response code="201">Event created successfully</response>
    /// <response code="400">Invalid request data or validation errors</response>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /api/Events
    ///     {
    ///         "name": "New Programming Contest",
    ///         "description": "A new exciting programming competition"
    ///     }
    /// 
    /// Sample response:
    /// 
    ///     {
    ///         "id": 4,
    ///         "name": "New Programming Contest",
    ///         "description": "A new exciting programming competition"
    ///     }
    /// </remarks>
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a new event",
        Description = "Creates a new event with the provided information."
    )]
    [SwaggerResponse(201, "Event created successfully", typeof(EventDto))]
    [SwaggerResponse(400, "Bad request - validation errors")]
    [ProducesResponseType(typeof(EventDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EventDto>> Create(
        [FromBody] CreateEventRequest request, 
        CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var result = await _eventService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Update an existing event
    /// </summary>
    /// <param name="id">The unique identifier of the event to update</param>
    /// <param name="request">The event update request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated event</returns>
    /// <response code="200">Event updated successfully</response>
    /// <response code="400">Invalid request data or ID mismatch</response>
    /// <response code="404">Event with the specified ID was not found</response>
    /// <remarks>
    /// Sample request:
    /// 
    ///     PUT /api/Events/1
    ///     {
    ///         "id": 1,
    ///         "name": "Updated Event Name",
    ///         "description": "Updated description"
    ///     }
    /// 
    /// Sample response:
    /// 
    ///     {
    ///         "id": 1,
    ///         "name": "Updated Event Name",
    ///         "description": "Updated description"
    ///     }
    /// </remarks>
    [HttpPut("{id}")]
    [SwaggerOperation(
        Summary = "Update an event",
        Description = "Updates an existing event with the provided information."
    )]
    [SwaggerResponse(200, "Event updated successfully", typeof(EventDto))]
    [SwaggerResponse(400, "Bad request - validation errors or ID mismatch")]
    [SwaggerResponse(404, "Event not found")]
    [ProducesResponseType(typeof(EventDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EventDto>> Update(
        int id, 
        [FromBody] UpdateEventRequest request, 
        CancellationToken cancellationToken)
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
            var result = await _eventService.UpdateAsync(request, cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Delete an event
    /// </summary>
    /// <param name="id">The unique identifier of the event to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>No content</returns>
    /// <response code="204">Event deleted successfully</response>
    /// <response code="404">Event with the specified ID was not found</response>
    /// <remarks>
    /// Sample request:
    /// 
    ///     DELETE /api/Events/1
    /// 
    /// Sample response:
    /// 
    ///     204 No Content
    /// </remarks>
    [HttpDelete("{id}")]
    [SwaggerOperation(
        Summary = "Delete an event",
        Description = "Deletes an event by its unique identifier."
    )]
    [SwaggerResponse(204, "Event deleted successfully")]
    [SwaggerResponse(404, "Event not found")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _eventService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
