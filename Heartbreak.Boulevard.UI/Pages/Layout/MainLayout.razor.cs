using Heartbreak.Boulevard.UI.Integration;
using Heartbreak.Boulevard.UI.Session;
using Microsoft.AspNetCore.Components;

namespace Heartbreak.Boulevard.UI.Pages.Layout;

public partial class MainLayout
{
    private HBBSession? _session;

    [Inject]
    public IHBBGitHubClient GithubClient { get; set; }


    protected override async Task OnParametersSetAsync()
    {
        if(_session == null)
        {
            var loaded = await GithubClient.LoadChapterEntries();
            _session = new HBBSession(loaded);
        }
    }



}
