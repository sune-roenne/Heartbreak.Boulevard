using Heartbreak.Boulevard.UI.Integration.Story;

namespace Heartbreak.Boulevard.UI.Integration;

public interface IHBBGitHubClient
{
    Task<IReadOnlyCollection<HBBChapterEntry>> LoadChapterEntries();

}
