using Microsoft.Azure.EventGrid.Models;


/// <summary>
/// /// Represents a service for handling Event Grid events.
/// /// </summary>
public interface IEventGridService
{
    /// <summary>
    /// Handles the specified Event Grid event asynchronously.
    /// </summary>
    /// <param name="eventGridEvent">The Event Grid event to handle.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task HandleEventAsync(EventGridEvent eventGridEvent);
}

/// <summary>
/// Provides an implementation of the <see cref="IEventGridService"/> interface.
/// </summary>
public class EventGridService : IEventGridService
{
    /// <inheritdoc/>
    public Task HandleEventAsync(EventGridEvent eventGridEvent)
    {
        return Task.CompletedTask;
    }
}