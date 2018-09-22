using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreparationMenu : UIScreen
{

    [Space, Header("Screens to switch to")]
    [SerializeField]
    private SO_Tag _DishScreenTag;

    public void OnClickDish()
    {
        _UIManager.SetScreen(_DishScreenTag);
    }

}
