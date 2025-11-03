using ContestService.Application.DTOs.Attachment;
using ContestService.Application.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ContestService.API.Controllers;

/// <summary>
/// Controller for managing Attachments
/// </summary>
[ApiController]
[Route("api/[controller]")]
[SwaggerTag("Operations for managing file attachments and documents")]
public class AttachmentsController : ControllerBase
{
    private readonly IAttachmentService _service;
    private readonly IValidator<CreateAttachmentRequest> _createValidator;
    private readonly IValidator<UpdateAttachmentRequest> _updateValidator;

    public AttachmentsController(
        IAttachmentService service,
        IValidator<CreateAttachmentRequest> createValidator,
        IValidator<UpdateAttachmentRequest> updateValidator)
    {
        _service = service;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    /// <summary>
    /// Get an attachment by ID
    /// </summary>
    /// <param name="id">The unique identifier of the attachment</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The attachment details</returns>
    /// <response code="200">Returns the attachment successfully</response>
    /// <response code="404">Attachment with the specified ID was not found</response>
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get attachment by ID", Description = "Retrieves a specific attachment by its unique identifier.")]
    [SwaggerResponse(200, "Attachment retrieved successfully", typeof(AttachmentDto))]
    [SwaggerResponse(404, "Attachment not found")]
    [ProducesResponseType(typeof(AttachmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AttachmentDto>> GetById(int id, CancellationToken cancellationToken)
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
    /// Get all attachments
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of attachments</returns>
    /// <response code="200">Returns the list of attachments successfully</response>
    [HttpGet]
    [SwaggerOperation(Summary = "Get all attachments", Description = "Retrieves all attachments in the system.")]
    [SwaggerResponse(200, "Attachments retrieved successfully", typeof(IEnumerable<AttachmentDto>))]
    [ProducesResponseType(typeof(IEnumerable<AttachmentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AttachmentDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _service.GetAllAsync(cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Create a new attachment
    /// </summary>
    /// <param name="request">The attachment creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created attachment</returns>
    /// <response code="201">Attachment created successfully</response>
    /// <response code="400">Invalid request data or validation errors</response>
    [HttpPost]
    [SwaggerOperation(Summary = "Create a new attachment", Description = "Creates a new attachment with the provided information.")]
    [SwaggerResponse(201, "Attachment created successfully", typeof(AttachmentDto))]
    [SwaggerResponse(400, "Bad request - validation errors")]
    [ProducesResponseType(typeof(AttachmentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AttachmentDto>> Create([FromBody] CreateAttachmentRequest request, CancellationToken cancellationToken)
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
    /// Update an existing attachment
    /// </summary>
    /// <param name="id">The unique identifier of the attachment to update</param>
    /// <param name="request">The attachment update request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated attachment</returns>
    /// <response code="200">Attachment updated successfully</response>
    /// <response code="400">Invalid request data or ID mismatch</response>
    /// <response code="404">Attachment with the specified ID was not found</response>
    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update an attachment", Description = "Updates an existing attachment with the provided information.")]
    [SwaggerResponse(200, "Attachment updated successfully", typeof(AttachmentDto))]
    [SwaggerResponse(400, "Bad request - validation errors or ID mismatch")]
    [SwaggerResponse(404, "Attachment not found")]
    [ProducesResponseType(typeof(AttachmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AttachmentDto>> Update(int id, [FromBody] UpdateAttachmentRequest request, CancellationToken cancellationToken)
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
    /// Delete an attachment
    /// </summary>
    /// <param name="id">The unique identifier of the attachment to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>No content</returns>
    /// <response code="204">Attachment deleted successfully</response>
    /// <response code="404">Attachment with the specified ID was not found</response>
    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete an attachment", Description = "Deletes an attachment by its unique identifier.")]
    [SwaggerResponse(204, "Attachment deleted successfully")]
    [SwaggerResponse(404, "Attachment not found")]
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
