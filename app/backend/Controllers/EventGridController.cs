using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc;
using MinimalApi.Hubs;
using MinimalApi.Models;
// Remove this using directive

namespace MinimalApi.Controllers;

[Route("api/[controller]")]
public class EventGridController : Controller
{
    #region Data Members

    private bool EventTypeSubcriptionValidation
        => HttpContext.Request.Headers["aeg-event-type"].FirstOrDefault() ==
           "SubscriptionValidation";

    private bool EventTypeNotification
        => HttpContext.Request.Headers["aeg-event-type"].FirstOrDefault() ==
           "Notification";

    private readonly IHubContext<GridEventsHub> _hubContext;

    #endregion

    #region Constructors

    public EventGridController(IHubContext<GridEventsHub> gridEventsHubContext)
    {
        this._hubContext = gridEventsHubContext;
    }

    #endregion

    #region Public Methods

    [HttpOptions]
    public Task<IActionResult> OptionsAsync()
    {
        using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
        {
            var webhookRequestOrigin = HttpContext.Request.Headers["WebHook-Request-Origin"].FirstOrDefault();
            var webhookRequestCallback = HttpContext.Request.Headers["WebHook-Request-Callback"];
            var webhookRequestRate = HttpContext.Request.Headers["WebHook-Request-Rate"];
            HttpContext.Response.Headers.Append("WebHook-Allowed-Rate", "*");
            HttpContext.Response.Headers.Append("WebHook-Allowed-Origin", webhookRequestOrigin);
        }

        return Task.FromResult<IActionResult>(Ok());
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync()
    {
        using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
        {
            var jsonContent = await reader.ReadToEndAsync();

            // Check the event type.
            // Return the validation code if it's 
            // a subscription validation request. 
            if (EventTypeSubcriptionValidation)
            {
                return await HandleValidationAsync(jsonContent);
            }
            else if (EventTypeNotification)
            {
                // Check to see if this is passed in using
                // the CloudEvents schema
                if (IsCloudEvent(jsonContent))
                {
                    return await HandleCloudEventAsync(jsonContent);
                }

                return await HandleGridEventsAsync(jsonContent);
            }

            return new JsonResult(BadRequest());
        }
    }

    #endregion

    #region Private Methods

    private async Task<JsonResult> HandleValidationAsync(string jsonContent)
    {
        var gridEvent =
            JsonConvert.DeserializeObject<List<GridEvent<Dictionary<string, string>>>>(jsonContent)
                ?.FirstOrDefault();
        if (gridEvent == null)
        {
            // Handle the case when the list is empty or null.
            return Json(BadRequest());
        }

        await this._hubContext.Clients.All.SendAsync(
            "gridupdate",
            gridEvent.Id,
            gridEvent.EventType,
            gridEvent.Subject,
            gridEvent.Data?["EventTime"]?.ToString(),
            jsonContent.ToString());

        // Retrieve the validation code and echo back.
        var validationCode = gridEvent.Data?["validationCode"];
        return new JsonResult(new
        {
            validationResponse = validationCode
        });
    }

    private async Task<IActionResult> HandleGridEventsAsync(string jsonContent)
    {
        var events = JArray.Parse(jsonContent);
        foreach (var e in events)
        {
            // Invoke a method on the clients for 
            // an event grid notiification.                        
            var details = JsonConvert.DeserializeObject<GridEvent<dynamic>>(e.ToString());
            await this._hubContext.Clients.All.SendAsync(
                "gridupdate",
                details?.Id,
                details?.EventType,
                details?.Subject,
                details?.EventTime.ToString("T"),
                e.ToString());
        }

        return Ok();
    }

    private async Task<IActionResult> HandleCloudEventAsync(string jsonContent)
    {
        var details = JsonConvert.DeserializeObject<CloudEvent<dynamic>>(jsonContent);
        var eventData = JObject.Parse(jsonContent);

        await this._hubContext.Clients.All.SendAsync(
            "gridupdate",
            details?.Id,
            details?.Type,
            details?.Subject,
            details?.Time,
            eventData.ToString()
        );

        return Ok();
    }

    private static bool IsCloudEvent(string jsonContent)
    {
        // Cloud events are sent one at a time, while Grid events
        // are sent in an array. As a result, the JObject.Parse will 
        // fail for Grid events. 
        try
        {
            // Attempt to read one JSON object. 
            var eventData = JObject.Parse(jsonContent);

            // Check for the spec version property.
            var version = eventData["specversion"]?.Value<string>();
            if (!string.IsNullOrEmpty(version))
            {
                return true;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return false;
    }

    #endregion
}