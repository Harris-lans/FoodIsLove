using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookingStationUI : MonoBehaviour 
{
    #region Member Variables 

        [Header("Events To Invoke")]
        [SerializeField]
        private SO_GenericEvent _OnStationPopupClickedEvent;

        private SpriteRenderer _StateImage;
        private CookingStation _CookingStation;
        private Animator _Animator;
        private Sprite _DefaultCookingStationIcon;
    
    #endregion

    #region Life Cycle

        private void Start()
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
            if (_CookingStation.IsAvailable || _CookingStation.IsCookedAndReady)
            {
                _OnStationPopupClickedEvent.Invoke(_CookingStation);
            }
        }

        public void DisplayCookingStationIcon()
        {
            _StateImage.sprite = _DefaultCookingStationIcon;
        }

    #endregion
}