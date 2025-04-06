using Booktex.Domain.Util;
using Heartbreak.Boulevard.UI.Pages.Layout;
using Microsoft.AspNetCore.Components;

namespace Heartbreak.Boulevard.UI.Components.Board;

public partial class PostItComponent
{

    [CascadingParameter]
    public HBBCommunication Communication { get; set; }

    [Parameter]
    public bool IsInHand { get; set; }

    [Parameter]
    public HBBPostItContent Content { get; set; }

    private int PostItNoToUse => ((Content.PostItTypeNo  - 1) % 4) +1;

    private string HandOrBoardString => IsInHand ? "hand" : "board";
    private string BackdropClass => $"hbb-board-postit-backdrop-{HandOrBoardString}";

    private string PostItClass => $"hbb-board-postit-{HandOrBoardString}";
    private string BackgroundClass => $"hbb-board-postit-bg-{HandOrBoardString}-{PostItNoToUse}";
    private string ContentClass => $"hbb-board-postit-content-{PostItNoToUse}";

    private string UnderfingerClass =>IsInHand ? 
        $"hbb-board-postit-underfinger-{PostItNoToUse}" :
        "";

    private string ThumbClass => IsInHand ?
        $"hbb-board-postit-thumb-{PostItNoToUse}" :
        "";

    private string ColorImage => (Content.ContentType switch {
        PostItContentType.MainCaseMurder => "murder",
        PostItContentType.MainCaseEvent => "event",
        PostItContentType.OtherMurder => "otmurder",
        _ => "otevent"
        } + $"-{PostItNoToUse}")
        .Pipe(_ => $"images/board/postit-{_}.webp" );


    private void OnBackdropClicked()
    {
        if (IsInHand)
        {
            Communication.ChangeSelectedPostIt(null);
        }
    }


}
