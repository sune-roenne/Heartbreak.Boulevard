namespace Heartbreak.Boulevard.UI.Components.Board;

public record HBBPostItContent(
    string Title,
    PostItContentType ContentType,
    int PostItTypeNo = 1,
    IReadOnlyCollection<(string Name, string Value)>? BodyDescriptions = null,
    IReadOnlyCollection<(string Name, string Value)>? BodyNameValues = null,
    IReadOnlyCollection<(string Name, string Value)>? FooterNameValues = null,
    IReadOnlyCollection<(string Name, string Value)>? FooterDescriptions = null
    )
{
    private static readonly object _idLock = new();
    private static long _currentId;
    private static long NextId()
    {
        lock (_idLock)
        {
            return _currentId++;
        }

    }

    public readonly long ContentId = NextId();


}
