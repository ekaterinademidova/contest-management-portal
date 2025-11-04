namespace ContestService.Application.DTOs.Event;

/// <summary>
/// Request model for updating an existing event
/// </summary>
/// <example>
/// {
///     "id": 1,
///     "name": "Updated Event Name",
///     "description": "Updated description"
/// }
/// </example>
public class UpdateEventRequest
{
    /// <summary>
    /// Unique identifier of the event to update
    /// </summary>
    /// <example>1</example>
    public int Id { get; set; }
    
    /// <summary>
    /// Updated name of the event (required, max 255 characters)
    /// </summary>
    /// <example>Updated Event Name</example>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Updated description of the event (optional)
    /// </summary>
    /// <example>Updated description</example>
    public string? Description { get; set; }
}
