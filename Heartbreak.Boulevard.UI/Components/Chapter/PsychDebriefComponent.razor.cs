using Booktex.Domain.Book.Specials.Model;
using Booktex.Domain.Util;
using Heartbreak.Boulevard.UI.Story;
using Microsoft.AspNetCore.Components;

namespace Heartbreak.Boulevard.UI.Components.Chapter;

public partial class PsychDebriefComponent
{
    [Parameter]
    public PsychDebrief Debrief { get; set; }

    private IReadOnlyCollection<PsychLogEntry>[]? _split;

    private string DateText => Debrief.Log.Date.ToString("dd-MM-yyyy");
    private string? TimeText => Debrief.Log.Time;
    private string? PlaceText => Debrief.Log.Place;
    private string PatientNoText => Debrief.Log.PatientNo;
    private string? PatientNameText => Debrief.Log.PatientName;

    private bool PatientNameIsRedacted => PatientNameText == null;



    protected override void OnParametersSet()
    {
        if(_split == null)
        {
            Split();
            _ = InvokeAsync(StateHasChanged);
        }
    }


    private void Split()
    {
        _split = [Debrief.Log.Entries.ToReadonlyCollection()];


    }





}
