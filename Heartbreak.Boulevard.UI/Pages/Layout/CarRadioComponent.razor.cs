using Booktex.Domain.Util;
using Microsoft.AspNetCore.Components;

namespace Heartbreak.Boulevard.UI.Pages.Layout;

public partial class CarRadioComponent
{
    [Parameter]
    public bool PlayerCanPlay { get; set; }

    [Parameter]
    public Action<bool> OnPowerStatusChanged { get; set; }

    [Parameter]
    public Action<bool> OnIsEjectedChanged { get; set; }

    private bool? _isOn = null;
    private bool _isEjected = false;

    private string PowerButtonClass => _isOn switch
    {
        null => "un-powered",
        true => "power-on",
        false => "power-off"

    } ;

    private void OnPowerClick()
    {
        if (PlayerCanPlay)
        {
            _isOn = _isOn?.Pipe(_ => !_) ?? true;
            OnPowerStatusChanged(_isOn.Value);
        }
    }

    private void OnEjectClick()
    {
        if (PlayerCanPlay)
        {
            _isEjected = !_isEjected;
            OnIsEjectedChanged(_isEjected);
        }
    }

}
