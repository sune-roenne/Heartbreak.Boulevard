using Heartbreak.Boulevard.UI.Pages.Layout;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Heartbreak.Boulevard.UI.Components;

public partial class SpotifyPlayerComponent : IDisposable
{
    private static long _currentId = 0L;
    private static readonly object _idLock = new { };
    private static long NextId()
    {
        lock(_idLock)
            return _currentId++;
    }
    private readonly long _id = NextId();

    private string PlayerId => $"hbb-spotify-player-div";
    private bool _didInitPlayer = false;

    [Inject]
    public IJSRuntime JS { get; set; }

    [Parameter]
    public string PlaylistId { get; set; }

    [Parameter]
    public TimeSpan? InitDelay { get; set; }

    [Parameter]
    public int ExpandedWidth { get; set; }

    [Parameter]
    public int Height { get; set; }

    [CascadingParameter]
    public HBBCommunication Communications { get; set; }

    private bool _registeredForCommunications = false;

    protected override void OnParametersSet()
    {
        if(!_registeredForCommunications)
        {
            Communications.OnRadioIsEjectedChanged += OnRadioIsEjectedChanged;
            _registeredForCommunications = true;
        }
    }


    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(!_didInitPlayer)
        {
            _didInitPlayer = true;
            await JS.InvokeVoidAsync("initSpotifyPlayer", PlayerId, PlaylistId);
            await JS.InvokeVoidAsync("setDimensions", ExpandedWidth, Height);

        }
    }

    private void OnRadioIsEjectedChanged(object? sender, bool? ejected)
    {
        if(_didInitPlayer)
        {
            var newWidth = (ejected ?? false) ? ExpandedWidth : 0;
            var height = Height;
            _ = Task.Run(async () =>
            {
                await JS.InvokeVoidAsync("setDimensions", newWidth, height);
            });

        }
    }

    public void Dispose()
    {
        if(_registeredForCommunications)
            Communications.OnRadioIsEjectedChanged -= OnRadioIsEjectedChanged;
    }
}
