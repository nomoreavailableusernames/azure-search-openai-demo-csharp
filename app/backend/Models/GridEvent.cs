/// <summary>
/// Represents a grid event.
/// </summary>
/// <typeparam name="T">The type of data associated with the event.</typeparam>
public class GridEvent<T> where T : class
{
    /// <summary>
    /// Gets or sets the ID of the event.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the type of the event.
    /// </summary>
    public string? EventType { get; set; }

    /// <summary>
    /// Gets or sets the subject of the event.
    ///     /// </summary>
    public string? Subject { get; set; }

    /// <summary>
    /// Gets or sets the time of the event.
    /// </summary>
    public DateTime EventTime { get; set; }

    /// <summary>
    /// Gets or sets the data associated with the event.
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Gets or sets the topic of the event.
    /// </summary>
    public string? Topic { get; set; }
}