using Booktex.Domain.Book.Model;

namespace Heartbreak.Boulevard.UI.Integration.Story;

public record HBBChapterEntry(
    HBBChapterSpecification Specification,
    IReadOnlyCollection<BookChapterContent>? Content
    );
