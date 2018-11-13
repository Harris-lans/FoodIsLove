using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class CombatOptionButton : MonoBehaviour 
{

    #region Member Variables

        [Header("Combat option to represent")]
        [SerializeField]
        private CombatOptions _CombatOptionToRepresent;

        private SO_CombatData _CombatData;
        private Button _Button;
        private Color _DisabledColor;
        private Color _NormalColor;

    #endregion

    #region Member Functions 

        private void Start()
        {
            _CombatData = Resources.Load<SO_CombatData>("CombatData");

            // Listening to both start and restart events in case of a draw between players
            _CombatData.CombatTimerStartedEvent.AddListener(OnCombatSequenceStarted);
            _CombatData.CombatTimerEndedEvent.AddListener(OnCombatOptionChosenLocally);
            // This event is not networked
            _CombatData.CombatOptionChosenLocallyEvent.AddListener(OnCombatOptionChosenLocally);

            _Button = GetComponent<Button>();
        }

        public void OnClick()
        {
            // Invoking the event with the player id and the option they have chosen
            LocalPlayerController localPlayerController = GameObject.FindObjectOfType<LocalPlayerController>();
            int[] data = { localPlayerController.GetComponent<PhotonView>().ViewID, (int) _CombatOptionToRepresent };
            _CombatData.CombatOptionChosenEvent.Invoke(data);
            _CombatData.CombatOptionChosenLocallyEvent.Invoke(data);
        }

        private void HighlightButton()
        {
            _Button.interactable = true;
        }

        private void DeHighlightButton()
        {
            _Button.interactable = false;
        }

        private void OnCombatSequenceStarted(object data)
        {
            HighlightButton();
        }

        private void OnCombatOptionChosenLocally(object data)
        {
            DeHighlightButton();
        }

    #endregion

    #region Custom Enums

        [System.Serializable]
        public enum CombatOptions : byte
        {
            ROCK = 0,
            PAPER,
            SCISSORS
        }

    #endregion
}


