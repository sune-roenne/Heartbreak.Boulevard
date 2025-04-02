using Booktex.Domain.Util;
using Microsoft.AspNetCore.Components;

namespace Heartbreak.Boulevard.UI.Pages.Layout;

public partial class CarRadioComponent
{
    [CascadingParameter]
    public HBBCommunication Communications { get; set; }


    private bool PlayerCanPlay => Communications.CanPlayRadio;
    private bool PlayerCanEject => Communications.CanEjectRadio;

    private bool RadioIsDisabled => !PlayerCanPlay;


    private string PowerButtonClass => Communications.RadioIsOn switch
    {
        null => "un-powered",
        true => "power-on",
        false => "power-off"

    } ;

    private void OnPowerClick()
    {
        if (PlayerCanPlay)
        {
            Communications.RadioIsOn = Communications.RadioIsOn?.Pipe(_ => !_) ?? true;
        }
    }

    private void OnEjectClick()
    {
        if (PlayerCanPlay)
        {
            Communications.RadioIsEjected = Communications.RadioIsEjected?.Pipe(_ => !_) ?? true;
        }
    }

}
