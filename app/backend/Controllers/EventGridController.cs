using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.EventGrid.Models;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class EventGridController : ControllerBase
{
    private readonly IEventGridService _eventGridService;

    public EventGridController(IEventGridService eventGridService)
    {
        _eventGridService = eventGridService;
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] EventGridEvent[] events)
    {
        foreach (var eventGridEvent in events)
        {
            await _eventGridService.HandleEventAsync(eventGridEvent);
        }
        return Ok();
    }
}