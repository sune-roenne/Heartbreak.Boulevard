using Heartbreak.Boulevard.UI.Components.Board;
using Heartbreak.Boulevard.UI.Integration;
using Heartbreak.Boulevard.UI.Integration.Story;
using Heartbreak.Boulevard.UI.Session;
using Microsoft.AspNetCore.Components;

namespace Heartbreak.Boulevard.UI.Pages.Layout;

public partial class MainLayout : IDisposable
{
    private HBBSession? _session;
    private HBBCommunication? _communications;
    private HBBPostItContent? _selectedPostIt;// = new HBBPostItContent("Title", ContentType: PostItContentType.OtherEvent, 4, [], [], [], []);

    [Inject]
    public IHBBGitHubClient GithubClient { get; set; }

    private bool _showChapterList = false;

    protected override async Task OnParametersSetAsync()
    {
        if(_session == null)
        {
            var loaded = await GithubClient.LoadChapterEntries();
            _session = new HBBSession(loaded);
        }
        if(_communications == null)
        {
            _communications = new HBBCommunication(
                UpdateUI: () => InvokeAsync(StateHasChanged)
            );
            _communications.OnPostItSelectionChanged += OnPostItSelectionChanged;
        }
    }

    private void OnChapterSelected(HBBChapterEntry chapter)
    {
        _showChapterList = false;
        _communications!.CurrentChapterEntry = chapter;
    }

    private void OnCloseClicked() => _communications!.CurrentChapterEntry = null;

    private string? PlayerEjectionClass => _communications?.RadioIsEjected switch
    {
        null => null,
        true => "player-eject",
        false => "player-uneject"
    };

    private void OnChaptersListButtonClicked()
    {
        _showChapterList = !_showChapterList;
        _ = InvokeAsync(StateHasChanged);
    }

    private void CloseReader()
    {
        _communications!.CurrentChapterEntry = null;
    }

    private void OnPostItSelectionChanged(object? sender, HBBPostItContent? newContent)
    {
        _selectedPostIt = newContent;
    }


    public void Dispose()
    {
        if(_communications != null)
        {
            _communications.OnPostItSelectionChanged -= OnPostItSelectionChanged;
        }
    }
}
