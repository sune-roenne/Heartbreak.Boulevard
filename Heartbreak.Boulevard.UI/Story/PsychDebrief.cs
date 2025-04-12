using Booktex.Domain.Book.Model;
using Booktex.Domain.Book.Specials.Model;

namespace Heartbreak.Boulevard.UI.Story;

public record PsychDebrief(
        PsychLog Log
    ) : BookChapterContent()
{
}
