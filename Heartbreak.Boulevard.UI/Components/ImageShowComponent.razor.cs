using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using SuneDoes.UI.Session;

namespace SuneDoes.UI.Components;

public partial class ImageShowComponent
{
    [CascadingParameter]
    public SessionState SessionState { get; set; }

    [Parameter]
    public bool UseDarkMode { get; set; } = false;


    [Parameter]
    public string Title { get; set; }

    [Parameter]
    public IEnumerable<ShowImage> Images { get; set; }
    private IReadOnlyCollection<ShowImage> _images = [];
    private ShowImage? _currentImage;

    protected override void OnParametersSet()
    {
        _images = Images.ToList();
        if(_images.Any())
            _currentImage = _images.First();
    }

    public static IReadOnlyCollection<ShowImage> From(params (string Location, string Name)[] images) => images
        .Select(_ => new ShowImage(_.Location, _.Name))
        .ToList();

    public record ShowImage(
        string Location,
        string Name
        );

    private void OnCloseClick(MouseEventArgs ev) => SessionState.StopShowImages();

    private void OnBackgroundClick(MouseEventArgs ev) => SessionState.StopShowImages();


}
