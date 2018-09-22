using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishMenu : UIScreen
{

    [Space, Header("Screens to switch to")]
    [SerializeField]
    private SO_Tag _PreparationScreenTag;

    public void OnClickClose()
    {
        _UIManager.SetScreen(_PreparationScreenTag);
    }

}
