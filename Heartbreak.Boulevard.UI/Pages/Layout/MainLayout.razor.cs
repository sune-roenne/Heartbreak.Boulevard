using Heartbreak.Boulevard.UI.Integration;
using Heartbreak.Boulevard.UI.Integration.Story;
using Heartbreak.Boulevard.UI.Session;
using Microsoft.AspNetCore.Components;

namespace Heartbreak.Boulevard.UI.Pages.Layout;

public partial class MainLayout
{
    private HBBSession? _session;

    [Inject]
    public IHBBGitHubClient GithubClient { get; set; }

    private HBBChapterEntry? _selectedChapter;

    protected override async Task OnParametersSetAsync()
    {
        if(_session == null)
        {
            var loaded = await GithubClient.LoadChapterEntries();
            _session = new HBBSession(loaded);
        }
    }

    private void OnChapterSelected(HBBChapterEntry chapter)
    {
        _selectedChapter = chapter;
        if(_selectedChapter.Specification.PlaylistId == null)
        {
            _playerIsEjected = null;
            _playerIsOn = null;
        }
        InvokeAsync(StateHasChanged);
    }

    private void OnCloseClicked()
    {
        if (_selectedChapter != null)
        {
            _playerIsEjected = null;
            _playerIsOn = null;
            _selectedChapter = null;
            InvokeAsync(StateHasChanged);
        }
    }

    private bool PlayerCanPlay => _selectedChapter?.Specification?.PlaylistId != null;

    private void OnPlayerEjectedChanged(bool? ejected)
    {
        _playerIsEjected = ejected;
        InvokeAsync(StateHasChanged);
    }
    private void OnPlayerIsOnChanged(bool isOn)
    {
        _playerIsOn = isOn;
        InvokeAsync(StateHasChanged);
    }
    private string? PlayerEjectionClass => _playerIsEjected switch
    {
        null => null,
        true => "player-eject",
        false => "player-uneject"
    };


    private bool? _playerIsEjected = null;
    private bool? _playerIsOn = null;



}
