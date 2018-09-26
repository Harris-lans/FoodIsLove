using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenMenu : UIScreen
{

    [Space, Header("Screens to switch to")]
    [SerializeField]
    private SO_Tag _MainMenuScreenTag;

    [Space, Header("Events to trigger")]
    [SerializeField]
    private SO_GenericEvent _TitleScreenMusicEvent;
    [SerializeField]
    private SO_GenericEvent _StopTitleScreenMusicEvent;

    protected override void Start()
    {
        base.Start();
        _TitleScreenMusicEvent.Invoke(null);
    }

    private void OnEnable()
    {
        // Checking the platform and optimizing the inputs accordingly
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            StartCoroutine(DetectMouseClicks());
        }
        else if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            StartCoroutine(DetectTouch());
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator DetectMouseClicks()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _UIManager.SetScreen(_MainMenuScreenTag);
                 _StopTitleScreenMusicEvent.Invoke(null);
            }

            yield return null;
        }
    }

    private IEnumerator DetectTouch()
    {
        while (true)
        {
            if (Input.touches.Length > 0)
            {
                _UIManager.SetScreen(_MainMenuScreenTag);
            }

            yield return null;
        }
    }
}
