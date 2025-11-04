namespace ContestService.Application.DTOs.Event;

/// <summary>
/// Request model for creating a new event
/// </summary>
/// <example>
/// {
///     "name": "New Programming Contest",
///     "description": "A new exciting programming competition"
/// }
/// </example>
public class CreateEventRequest
{
    /// <summary>
    /// Name of the event (required, max 255 characters)
    /// </summary>
    /// <example>New Programming Contest</example>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Description of the event (optional)
    /// </summary>
    /// <example>A new exciting programming competition</example>
    public string? Description { get; set; }
}
