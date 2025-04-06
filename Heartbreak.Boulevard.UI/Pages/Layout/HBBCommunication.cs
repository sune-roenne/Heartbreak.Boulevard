using Heartbreak.Boulevard.UI.Components.Board;
using Heartbreak.Boulevard.UI.Integration.Story;

namespace Heartbreak.Boulevard.UI.Pages.Layout;

public record HBBCommunication(
    Action UpdateUI
    
    )
{
    public event EventHandler<bool?> OnRadioPowerIsOnChanged;
    public event EventHandler<bool?> OnRadioIsEjectedChanged;
    public event EventHandler<HBBChapterEntry?> OnChapterSelectionChanged;
    public event EventHandler<HBBPostItContent?> OnPostItSelectionChanged;


    private bool? _radioIsOn = null;
    public bool? RadioIsOn
    {
        get => _radioIsOn;
        set {
            var onValueBeforeUpdate = _radioIsOn;
            _radioIsOn = value;
            OnRadioPowerIsOnChanged?.Invoke(this, _radioIsOn);
            if(_radioIsOn == false)
            {
                _radioIsEjected = null;
                OnRadioIsEjectedChanged?.Invoke(this, _radioIsEjected);
            }
            else if(_radioIsOn == true)
            {
                _radioIsEjected = true;
                OnRadioIsEjectedChanged?.Invoke(this, _radioIsEjected);
            }
            UpdateUI();
        }
    }
    
    private bool? _radioIsEjected = null;
    public bool? RadioIsEjected
    {
        get => _radioIsEjected;
        set
        {
            _radioIsEjected = value;
            OnRadioIsEjectedChanged?.Invoke(this, _radioIsEjected);
            UpdateUI();
        }
    }


    private HBBChapterEntry? _currentEntry;
    public HBBChapterEntry? CurrentChapterEntry
    {
        get => _currentEntry;
        set
        {
            var playlistIdBeforeUpdate = CurrentPlaylistId;
            _currentEntry = value;
            if(!string.IsNullOrWhiteSpace(playlistIdBeforeUpdate) && string.IsNullOrEmpty(CurrentPlaylistId))
            {
                OnChapterSelectionChanged?.Invoke(this, _currentEntry);
                RadioIsOn = null; // Assignment will update UI
            }
            else
            {
                OnChapterSelectionChanged?.Invoke(this, _currentEntry);
                UpdateUI();
            }
        }
    }


    public string? CurrentPlaylistId => _currentEntry?.Specification?.PlaylistId;

    public bool CanPlayRadio => CurrentPlaylistId != null;
    public bool CanEjectRadio => CanPlayRadio && _radioIsOn == true;

    private HBBPostItContent? _currentPostIt;

    public void ChangeSelectedPostIt(HBBPostItContent? postIt)
    {
        var callUpdate =
            ((postIt == null) != (_currentPostIt == null)) ||
            (postIt != null && _currentPostIt != null && postIt.ContentId != _currentPostIt.ContentId);
        _currentPostIt = postIt;
        if (callUpdate)
        {
            OnPostItSelectionChanged?.Invoke(this, _currentPostIt);
            UpdateUI();
        }
    }



}
