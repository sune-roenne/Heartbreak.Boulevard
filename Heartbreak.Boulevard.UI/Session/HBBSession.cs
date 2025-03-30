using Heartbreak.Boulevard.UI.Integration.Story;

namespace Heartbreak.Boulevard.UI.Session;

public record HBBSession(
    IReadOnlyCollection<HBBChapterEntry> Chapters
    
    )
{
}
