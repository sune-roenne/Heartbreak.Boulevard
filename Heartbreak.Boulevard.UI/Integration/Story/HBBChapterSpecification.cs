namespace Heartbreak.Boulevard.UI.Integration.Story;

public record HBBChapterSpecification(
    string ChapterName,
    string OrderKey,
    string? FileName,
    string? PlaylistId,
    bool CarryOverBefore = false,
    bool CarryOverAfter = false
    )
{
    public const string PsychFileNameEnding = "psych";

    public bool IsPsychFile => (FileName ?? "").ToLower().Trim().EndsWith(PsychFileNameEnding);
}
