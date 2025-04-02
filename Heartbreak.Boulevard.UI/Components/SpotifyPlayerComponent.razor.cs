using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Heartbreak.Boulevard.UI.Components;

public partial class SpotifyPlayerComponent
{
    private static long _currentId = 0L;
    private static readonly object _idLock = new { };
    private static long NextId()
    {
        lock(_idLock)
            return _currentId++;
    }
    private readonly long _id = NextId();

    private string PlayerId => $"hbb-spotify-player{_id}";
    private bool _didInitPlayer = false;

    [Inject]
    public IJSRuntime JS { get; set; }

    [Parameter]
    public string PlaylistId { get; set; }

    [Parameter]
    public TimeSpan? InitDelay { get; set; }

    [Parameter]
    public int Width { get; set; }

    [Parameter]
    public int Height { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(!_didInitPlayer)
        {
            _didInitPlayer = true;
            if (InitDelay != null)
                await Task.Delay(InitDelay.Value);
            await JS.InvokeVoidAsync("initSpotifyPlayer", PlayerId, PlaylistId);
            await JS.InvokeVoidAsync("setDimensions", Width, Height);

        }
    }
}
