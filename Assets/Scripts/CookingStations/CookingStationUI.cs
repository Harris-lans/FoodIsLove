using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookingStationUI : MonoBehaviour 
{
    #region Member Variables 

        [Header("Cooking Station State Details")]
        [SerializeField]
        private List<CookingStationStateDetails> _StateInformations;
        
        [Header("Events To Invoke")]
        [SerializeField]
        private SO_GenericEvent _OnStationPopupClickedEvent;

        private SpriteRenderer _StateImage;
        private CookingStation _CookingStation;
    
    #endregion

    #region Life Cycle

        private void Start()
        {
            _CookingStation = GetComponentInParent<CookingStation>();
            _StateImage = GetComponentInChildren<SpriteRenderer>();
            
            // Initial sprite update
            UpdateUI();
        }

    #endregion 

    #region Member Functions

        public void UpdateUI()
        {
            foreach (var state in _StateInformations)
            {
                if (state.State == _CookingStation.State)
                {
                    _StateImage.sprite = state.StateSprite;
                    return;
                }
            }
        }

        public void OnClicked()
        {
            if (_CookingStation.IsAvailable || _CookingStation.IsCookedAndReady)
            {
                _OnStationPopupClickedEvent.Invoke(_CookingStation);
            }
        }

    #endregion
}

[System.Serializable]
public struct CookingStationStateDetails
{
    public CookingStation.CookingStationState State;
    public Sprite StateSprite;
}