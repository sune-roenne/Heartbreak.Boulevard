using Booktex.Domain.Book.Model;

namespace Heartbreak.Boulevard.UI.Story;

public record ShrapnelOfLove(
    string SubTitle,
    IReadOnlyCollection<BookChapterContent> Content
    ) : BookChapterContent
{

}
