using Booktex.Domain.Book.Model;
using Booktex.Html.Common.Style;

namespace Heartbreak.Boulevard.UI.Story;

public static class QuoteBackgrounds
{
    public static BooktexBackgroundImageSpecification? BackgroundFor(this BookQuote quote) => quote.Name?.Trim()?.ToLower() switch
    {
        null => null,
        string nam when nam.Contains("cold war") => new BooktexBackgroundImageSpecification(
            BackgroundImage: "url('images/story/cold-war-bg.webp')",
            BackgroundSize: "contain",
            BackgroundRepeat: "no-repeat",
            BackgroundPositionX: "45%"
            ),
        _ => null
    };


}
