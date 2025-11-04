using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SubmissionService.Application.DTOs.Submission;
using SubmissionService.Application.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace SubmissionService.API.Controllers;

/// <summary>
/// Controller for managing Submissions
/// </summary>
[ApiController]
[Route("api/[controller]")]
[SwaggerTag("Operations for managing contest submissions")]
public class SubmissionsController : ControllerBase
{
    private readonly ISubmissionService _service;
    private readonly IValidator<CreateSubmissionRequest> _createValidator;
    private readonly IValidator<UpdateSubmissionRequest> _updateValidator;

    public SubmissionsController(
        ISubmissionService service,
        IValidator<CreateSubmissionRequest> createValidator,
        IValidator<UpdateSubmissionRequest> updateValidator)
    {
        _service = service;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    /// <summary>
    /// Get a submission by ID
    /// </summary>
    /// <param name="id">The unique identifier of the submission</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The submission details</returns>
    /// <response code="200">Returns the submission successfully</response>
    /// <response code="404">Submission with the specified ID was not found</response>
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get submission by ID", Description = "Retrieves a specific submission by its unique identifier.")]
    [SwaggerResponse(200, "Submission retrieved successfully", typeof(SubmissionDto))]
    [SwaggerResponse(404, "Submission not found")]
    [ProducesResponseType(typeof(SubmissionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SubmissionDto>> GetById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _service.GetSubmissionByIdAsync(id, cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Get all submissions with optional filtering
    /// </summary>
    /// <param name="filter">Optional filter parameters (contestNoticeId, participantId, submissionState)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of submissions</returns>
    /// <response code="200">Returns the list of submissions successfully</response>
    [HttpGet]
    [SwaggerOperation(Summary = "Get all submissions", Description = "Retrieves all submissions. Supports optional filtering by contestNoticeId, participantId, and submissionState.")]
    [SwaggerResponse(200, "Submissions retrieved successfully", typeof(IEnumerable<SubmissionDto>))]
    [ProducesResponseType(typeof(IEnumerable<SubmissionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SubmissionDto>>> GetAll([FromQuery] SubmissionFilterRequest? filter, CancellationToken cancellationToken)
    {
        var result = await _service.GetAllSubmissionsAsync(filter, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Create a new submission
    /// </summary>
    /// <param name="request">The submission creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created submission</returns>
    /// <response code="201">Submission created successfully</response>
    /// <response code="400">Invalid request data or validation errors</response>
    [HttpPost]
    [SwaggerOperation(Summary = "Create a new submission", Description = "Creates a new submission with the provided information.")]
    [SwaggerResponse(201, "Submission created successfully", typeof(SubmissionDto))]
    [SwaggerResponse(400, "Bad request - validation errors")]
    [ProducesResponseType(typeof(SubmissionDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SubmissionDto>> Create([FromBody] CreateSubmissionRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        try
        {
            var result = await _service.CreateSubmissionAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Update an existing submission
    /// </summary>
    /// <param name="id">The unique identifier of the submission to update</param>
    /// <param name="request">The submission update request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated submission</returns>
    /// <response code="200">Submission updated successfully</response>
    /// <response code="400">Invalid request data or ID mismatch</response>
    /// <response code="404">Submission with the specified ID was not found</response>
    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update a submission", Description = "Updates an existing submission with the provided information.")]
    [SwaggerResponse(200, "Submission updated successfully", typeof(SubmissionDto))]
    [SwaggerResponse(400, "Bad request - validation errors or ID mismatch")]
    [SwaggerResponse(404, "Submission not found")]
    [ProducesResponseType(typeof(SubmissionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SubmissionDto>> Update(int id, [FromBody] UpdateSubmissionRequest request, CancellationToken cancellationToken)
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
            var result = await _service.UpdateSubmissionAsync(request, cancellationToken);
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
    /// Delete a submission
    /// </summary>
    /// <param name="id">The unique identifier of the submission to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>No content</returns>
    /// <response code="204">Submission deleted successfully</response>
    /// <response code="404">Submission with the specified ID was not found</response>
    /// <response code="400">Cannot delete submission with associated appeal</response>
    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete a submission", Description = "Deletes a submission by its unique identifier. Cannot delete if an appeal exists.")]
    [SwaggerResponse(204, "Submission deleted successfully")]
    [SwaggerResponse(404, "Submission not found")]
    [SwaggerResponse(400, "Cannot delete submission with associated appeal")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _service.DeleteSubmissionAsync(id, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

