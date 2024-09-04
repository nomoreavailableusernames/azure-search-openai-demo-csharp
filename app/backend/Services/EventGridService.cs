using Microsoft.Azure.EventGrid.Models;
using System.Threading.Tasks;

public interface IEventGridService
{
    Task HandleEventAsync(EventGridEvent eventGridEvent);
}

public class EventGridService : IEventGridService
{

    public Task HandleEventAsync(EventGridEvent eventGridEvent)
    {
        return Task.CompletedTask;
    }
}