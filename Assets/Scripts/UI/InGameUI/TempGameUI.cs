using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempGameUI : UIScreen 
{
    [Space, Header("Screens to switch to")]
    [SerializeField]
    private SO_Tag _OptionsScreenTag;

    public void OnClickedOptions()
    {
        _UIManager.SetScreen(_OptionsScreenTag);
    }
}
