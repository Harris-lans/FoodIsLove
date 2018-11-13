using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookingStationUI : MonoBehaviour 
{
    #region Member Variables 

        [Space, Header("Icons")]
        [SerializeField]
        private Sprite _EmptyIcon;

        private SpriteRenderer _StateImage;
        private CookingStation _CookingStation;
        private Animator _Animator;
        private Sprite _DefaultCookingStationIcon;
    
    #endregion

    #region Life Cycle

        private void Awake()
        {
            _CookingStation = GetComponentInParent<CookingStation>();
            _StateImage = GetComponent<SpriteRenderer>();
            _Animator = GetComponent<Animator>();

            // Initial sprite update
            _DefaultCookingStationIcon = _StateImage.sprite;
            UpdateUI();
        }

    #endregion 

    #region Member Functions

        public void UpdateUI()
        {
            _Animator.SetInteger("State", (int)_CookingStation.State);
        }

        public void OnClicked()
        {
            _CookingStation.OnClickedOnNode();
        }

        public void DisplayCookingStationIcon()
        {
            _StateImage.sprite = _DefaultCookingStationIcon;
        }

    #endregion
}