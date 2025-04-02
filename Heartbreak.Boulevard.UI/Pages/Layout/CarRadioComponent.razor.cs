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
    public Action<bool?> OnIsEjectedChanged { get; set; }

    [Parameter]
    public bool? IsOn { get; set; }


    [Parameter]
    public bool? IsEjected { get; set; }


    private string PowerButtonClass => IsOn switch
    {
        null => "un-powered",
        true => "power-on",
        false => "power-off"

    } ;

    private void OnPowerClick()
    {
        if (PlayerCanPlay)
        {
            IsOn = IsOn?.Pipe(_ => !_) ?? true;
            OnPowerStatusChanged(IsOn.Value);
            if(IsEjected == true && IsOn == false)
            {
                IsEjected = null;
                OnIsEjectedChanged(null);
            }
        }
    }

    private void OnEjectClick()
    {
        if (PlayerCanPlay)
        {
            IsEjected = IsEjected?.Pipe(_ => !_) ?? true;
            OnIsEjectedChanged(IsEjected.Value);
        }
    }

}
