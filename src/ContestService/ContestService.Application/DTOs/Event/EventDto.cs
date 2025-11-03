namespace ContestService.Application.DTOs.Event;

/// <summary>
/// Event data transfer object
/// </summary>
/// <example>
/// {
///     "id": 1,
///     "name": "Annual Programming Championship 2024",
///     "description": "A prestigious programming competition bringing together the best coders from universities and tech companies."
/// }
/// </example>
public class EventDto
{
    /// <summary>
    /// Unique identifier of the event
    /// </summary>
    /// <example>1</example>
    public int Id { get; set; }
    
    /// <summary>
    /// Name of the event
    /// </summary>
    /// <example>Annual Programming Championship 2024</example>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Description of the event
    /// </summary>
    /// <example>A prestigious programming competition bringing together the best coders from universities and tech companies.</example>
    public string? Description { get; set; }
}
