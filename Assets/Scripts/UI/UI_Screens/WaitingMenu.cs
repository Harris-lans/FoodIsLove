using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingMenu : UIScreen
{

    [Space, Header("Screens to switch to")]
    [SerializeField]
    private SO_Tag _FoodWorldScreenTag;

    public void OnClickBackFoodWorld()
    {
        _UIManager.SetScreen(_FoodWorldScreenTag);
    }

}
