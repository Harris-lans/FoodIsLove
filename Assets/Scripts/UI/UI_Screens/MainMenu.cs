using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : UIScreen
{

    [Space, Header("Screens to switch to")]
    [SerializeField]
    private SO_Tag _MainOptionsScreenTag;
    [SerializeField]
    private SO_Tag _CreditsScreenTag;
    [SerializeField]
    private SO_Tag _FoodWorldScreenTag;

    [Space, Header("Events to invoke")]
    [SerializeField]
    private SO_GenericEvent _EnteredMainMenuEvent;

    [Header("UI Elements")]
    [SerializeField]
    private Image _OText;
    [SerializeField]
    private Button _OButton;
    [SerializeField]
    private Sprite _OTextOn;
    [SerializeField]
    private Sprite _OTextOff;
    [SerializeField]
    private Sprite _OButtonDisabled;
    [SerializeField]
    private Sprite _OButtonEnabled;

    private bool _MusicMuted;
    private AkAudioListener _AudioListener;

    #region Life Cycle

        private void Start()
        {
            _MusicMuted = false;
        }

        private void OnEnable()
        {
            _EnteredMainMenuEvent.Invoke(null);
            _AudioListener = GameObject.FindObjectOfType<AkAudioListener>();
        }

    #endregion

    public void OnClickedMainOptions()
    {
        _UIManager.SetScreen(_MainOptionsScreenTag);
    }

    public void OnClickedCredits()
    {
        _UIManager.SetScreen(_CreditsScreenTag);
    }

    public void OnClickedFoodWorld()
    {
        _UIManager.SetScreen(_FoodWorldScreenTag);
    }

    public void OnClickedExitGame()
    {
        Application.Quit();
    }

    public void ToggleMusic()
    {
        if (!_MusicMuted)
        {
            _AudioListener.enabled = false;
            _OButton.image.sprite = _OButtonDisabled;
            _OText.sprite = _OTextOff;
            _MusicMuted = true;
            return;
        }
        _AudioListener.enabled = true;
        _OButton.image.sprite = _OButtonEnabled;
        _OText.sprite = _OTextOn;
        _MusicMuted = false;
    }
}
