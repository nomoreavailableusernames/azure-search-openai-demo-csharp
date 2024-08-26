using Microsoft.AspNetCore.Components;

namespace SharedWebComponents.Components;

public partial class Loading : ComponentBase
{
    [Parameter]
    public string BusyMessage { get; set; } = "🤖 I'm generating an answer... Please wait.";
}