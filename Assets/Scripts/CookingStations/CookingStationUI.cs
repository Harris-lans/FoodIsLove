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

        private Image _StateImage;
        private CookingStation _CookingStation;
    
    #endregion

    #region Life Cycle

        private void Start()
        {
            _CookingStation = GetComponentInParent<CookingStation>();
            _StateImage = GetComponentInChildren<Image>();
            
            // Initial sprite update
            UpdateUI();
        }

    #endregion 

    #region Member Functions

        public void UpdateUI()
        {
            foreach (var state in _StateInformations)
            {
                Debug.Log(state.State);
                if (state.State == _CookingStation.State)
                {
                    Debug.Log(state.State);
                    Debug.Log(state.StateSprite.name);
                    _StateImage.sprite = state.StateSprite;
                    return;
                }
            }
        }

        public void OnClicked()
        {
            if (_CookingStation.IsAvailable)
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