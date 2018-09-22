using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenuScreen : UIScreen
{
    [Space, Header("Screens to switch to")]
    [SerializeField]
    private SO_Tag _MainMenuScreenTag;

    [SerializeField]
    private SO_Tag _InGameUITag;


    public void OnClickedExit()
    {
        _UIManager.SetScreen(_MainMenuScreenTag);
    }

    public void OnClickedDoneInGame()
    {
        _UIManager.SetScreen(_InGameUITag);
    }

    public void OnClickedDoneMenu()
    {
        _UIManager.SetScreen(_MainMenuScreenTag);
    }

}
