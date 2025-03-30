using Microsoft.AspNetCore.Components;

namespace Heartbreak.Boulevard.UI.Pages.Layout;

public partial class PoliceLightsComponent
{
    [Parameter]
    public string PositionLeft { get; set; }
    [Parameter]
    public string PositionTop { get; set; }
    [Parameter]
    public int ZIndex { get; set; }


}
