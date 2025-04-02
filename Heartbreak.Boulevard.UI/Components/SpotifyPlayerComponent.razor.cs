using Heartbreak.Boulevard.UI.Pages.Layout;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Heartbreak.Boulevard.UI.Components;

public partial class SpotifyPlayerComponent : IDisposable
{
    private string PlayerId => $"hbb-spotify-player-div";
    private bool _didInitPlayer = false;

    [Inject]
    public IJSRuntime JS { get; set; }

    [Parameter]
    public TimeSpan? InitDelay { get; set; }

    [Parameter]
    public int ExpandedWidth { get; set; }

    [Parameter]
    public int Height { get; set; }

    [CascadingParameter]
    public HBBCommunication Communications { get; set; }

    private bool _registeredForCommunications = false;
    private string? _currentPlaylistId = null;



    protected override async Task OnParametersSetAsync()
    {
        if(!_registeredForCommunications)
        {
            Communications.OnRadioIsEjectedChanged += OnRadioIsEjectedChanged;
            _registeredForCommunications = true;
        }
        if(Communications.CurrentPlaylistId != _currentPlaylistId)
        {
            await JS.InvokeVoidAsync("setPlaylistId", Communications.CurrentPlaylistId);
            _currentPlaylistId = Communications.CurrentPlaylistId;
        }
    }


    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(!_didInitPlayer)
        {
            _didInitPlayer = true;
            await JS.InvokeVoidAsync("initSpotifyPlayer", PlayerId);
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
        {
            Communications.OnRadioIsEjectedChanged -= OnRadioIsEjectedChanged;
            _registeredForCommunications = false;
        }
    }

    private record CurrentlyPlayTrackInfo(string? PlaylistId, string TrackName);


}
