using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SubmissionService.Application.DTOs.Appeal;
using SubmissionService.Application.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace SubmissionService.API.Controllers;

/// <summary>
/// Controller for managing Appeals
/// </summary>
[ApiController]
[Route("api/[controller]")]
[SwaggerTag("Operations for managing appeals on submissions")]
public class AppealsController : ControllerBase
{
    private readonly ISubmissionService _service;
    private readonly IValidator<CreateAppealRequest> _createValidator;
    private readonly IValidator<UpdateAppealRequest> _updateValidator;

    public AppealsController(
        ISubmissionService service,
        IValidator<CreateAppealRequest> createValidator,
        IValidator<UpdateAppealRequest> updateValidator)
    {
        _service = service;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    /// <summary>
    /// Get an appeal by ID
    /// </summary>
    /// <param name="id">The unique identifier of the appeal</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The appeal details</returns>
    /// <response code="200">Returns the appeal successfully</response>
    /// <response code="404">Appeal with the specified ID was not found</response>
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get appeal by ID", Description = "Retrieves a specific appeal by its unique identifier.")]
    [SwaggerResponse(200, "Appeal retrieved successfully", typeof(AppealDto))]
    [SwaggerResponse(404, "Appeal not found")]
    [ProducesResponseType(typeof(AppealDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AppealDto>> GetById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _service.GetAppealByIdAsync(id, cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Get an appeal by submission ID
    /// </summary>
    /// <param name="submissionId">The unique identifier of the submission</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The appeal details if exists</returns>
    /// <response code="200">Returns the appeal successfully</response>
    /// <response code="404">No appeal found for the specified submission</response>
    [HttpGet("submission/{submissionId}")]
    [SwaggerOperation(Summary = "Get appeal by submission ID", Description = "Retrieves an appeal associated with a specific submission. Returns 404 if no appeal exists.")]
    [SwaggerResponse(200, "Appeal retrieved successfully", typeof(AppealDto))]
    [SwaggerResponse(404, "No appeal found for the submission")]
    [ProducesResponseType(typeof(AppealDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AppealDto>> GetBySubmissionId(int submissionId, CancellationToken cancellationToken)
    {
        var result = await _service.GetAppealBySubmissionIdAsync(submissionId, cancellationToken);
        if (result == null)
        {
            return NotFound($"No appeal found for submission with ID {submissionId}.");
        }
        return Ok(result);
    }

    /// <summary>
    /// Get all appeals
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of appeals</returns>
    /// <response code="200">Returns the list of appeals successfully</response>
    [HttpGet]
    [SwaggerOperation(Summary = "Get all appeals", Description = "Retrieves all appeals in the system.")]
    [SwaggerResponse(200, "Appeals retrieved successfully", typeof(IEnumerable<AppealDto>))]
    [ProducesResponseType(typeof(IEnumerable<AppealDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AppealDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _service.GetAllAppealsAsync(cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Create a new appeal
    /// </summary>
    /// <param name="request">The appeal creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created appeal</returns>
    /// <response code="201">Appeal created successfully</response>
    /// <response code="400">Invalid request data, validation errors, or appeal already exists</response>
    [HttpPost]
    [SwaggerOperation(Summary = "Create a new appeal", Description = "Creates a new appeal for a submission. Only one appeal per submission is allowed.")]
    [SwaggerResponse(201, "Appeal created successfully", typeof(AppealDto))]
    [SwaggerResponse(400, "Bad request - validation errors or appeal already exists")]
    [ProducesResponseType(typeof(AppealDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AppealDto>> Create([FromBody] CreateAppealRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        try
        {
            var result = await _service.CreateAppealAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Update an existing appeal
    /// </summary>
    /// <param name="id">The unique identifier of the appeal to update</param>
    /// <param name="request">The appeal update request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated appeal</returns>
    /// <response code="200">Appeal updated successfully</response>
    /// <response code="400">Invalid request data or ID mismatch</response>
    /// <response code="404">Appeal with the specified ID was not found</response>
    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update an appeal", Description = "Updates an existing appeal with the provided information.")]
    [SwaggerResponse(200, "Appeal updated successfully", typeof(AppealDto))]
    [SwaggerResponse(400, "Bad request - validation errors or ID mismatch")]
    [SwaggerResponse(404, "Appeal not found")]
    [ProducesResponseType(typeof(AppealDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AppealDto>> Update(int id, [FromBody] UpdateAppealRequest request, CancellationToken cancellationToken)
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
            var result = await _service.UpdateAppealAsync(request, cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Delete an appeal
    /// </summary>
    /// <param name="id">The unique identifier of the appeal to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>No content</returns>
    /// <response code="204">Appeal deleted successfully</response>
    /// <response code="404">Appeal with the specified ID was not found</response>
    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete an appeal", Description = "Deletes an appeal by its unique identifier.")]
    [SwaggerResponse(204, "Appeal deleted successfully")]
    [SwaggerResponse(404, "Appeal not found")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _service.DeleteAppealAsync(id, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}

