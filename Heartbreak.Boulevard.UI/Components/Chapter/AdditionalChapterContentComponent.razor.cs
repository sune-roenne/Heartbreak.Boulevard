using Booktex.Domain.Book.Model;
using Microsoft.AspNetCore.Components;

namespace Heartbreak.Boulevard.UI.Components.Chapter;

public partial class AdditionalChapterContentComponent
{
    [Parameter]
    public BookChapterContent Content { get; set; }

    private int _dialogIndx = 0;
    private int NextDialogIndex()
    {
        _dialogIndx = (_dialogIndx + 1) % 4;
        return _dialogIndx;
    }
}
