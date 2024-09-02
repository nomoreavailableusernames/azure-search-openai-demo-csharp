using Microsoft.Azure.EventGrid.Models;
using System.Threading.Tasks;

public interface IEventGridService
{
    Task HandleEventAsync(EventGridEvent eventGridEvent);
}

public class EventGridService : IEventGridService
{
    private readonly MainLayout _mainLayout;

    public EventGridService(MainLayout mainLayout)
    {
        _mainLayout = mainLayout;
    }

    public Task HandleEventAsync(EventGridEvent eventGridEvent)
    {
        // Example: Check event type and trigger Snackbar
        if (eventGridEvent.EventType == "YourEventType")
        {
            _mainLayout.ShowSnackbar();
        }
        return Task.CompletedTask;
    }
}