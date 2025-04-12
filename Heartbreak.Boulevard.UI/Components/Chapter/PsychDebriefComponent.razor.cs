using Booktex.Domain.Book.Specials.Model;
using Booktex.Domain.Util;
using Heartbreak.Boulevard.UI.Story;
using Microsoft.AspNetCore.Components;
using System.Text.RegularExpressions;

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

    private static string EntryContainerEmphasisClass(PsychLogEntry ent) => ent.EntryImportance switch {
        PsychLogEntryImportance.VeryHigh => "entry-container-high-emphasis",
        PsychLogEntryImportance.High => "entry-container-emphasis",
        _ => ""
    };


    private static readonly Regex RedactedRegex = new Regex(@"\[redacted(\([0-9]+\))?\]", RegexOptions.IgnoreCase);

    private static IReadOnlyCollection<CharacterBlock> SplitContent(string content)
    {
        var returnee = new List<CharacterBlock>();
        var matches = RedactedRegex.Matches(content).ToList();
        var startIndex = 0;
        foreach (var match in matches)
        {
            if(match.Index > startIndex)
            {
                var before = content.Substring(startIndex, match.Index - startIndex);
                before = before.Trim();
                if(before.Length > 0) 
                    returnee.Add(new CharacterBlock(Words: before));
            }
            var redactedCharacters = 8;
            if(match.Groups.Count > 1)
            {
                var redactedString = match.Groups[1].Value
                    .Replace("(", "")
                    .Replace(")", "");
                if (int.TryParse(redactedString, out var redChars))
                    redactedCharacters = redChars;
            }
            returnee.Add(new CharacterBlock(IsRedacted: true, CharactersRedacted: redactedCharacters));
            startIndex = match.Index + match.Length + 1;
        }
        if(startIndex < content.Length)
            returnee.Add(new CharacterBlock(Words: content.Substring(startIndex, content.Length - startIndex).Trim()));
        return returnee;
    }

    private static string QuoteAbout(PsychLogQuote quot) => new List<string?> { quot.MadeBy, quot.Context }
        .Where(_ => _ != null)
        .MakeString(" - ");

    private static bool IncludeType(PsychLogEntry ent) => ent switch
    {
        PsychLogEntry _ when ent.EntryImportance >= PsychLogEntryImportance.High => true,
        _ => false
    };

    private static IReadOnlyCollection<CharacterBlock> SplitToBlocks(PsychLogEntry ent) => ent switch
    {
        PsychLogObservation obs => obs.Lines
           .SelectMany(SplitContent)
           .ToReadonlyCollection(),
        PsychLogReflection obs => obs.Lines
           .SelectMany(SplitContent)
           .ToReadonlyCollection(),
        _ => []
    };

    private static string TableEntryType(PsychLogEntry ent) => ent switch
    {
        PsychLogObservation obs => "Observation",
        PsychLogReflection refl => "Reflection",
        _ => ""
    };


    private record CharacterBlock(
        string? Words = null,
        bool IsRedacted = false,
        int? CharactersRedacted = null
        )
    {
        public IReadOnlyCollection<string> GetWordStrings => Words?.Split(' ') ?? 
            [Enumerable.Range(0, CharactersRedacted!.Value).Select(_ => "X").MakeString("")];
    }





}
