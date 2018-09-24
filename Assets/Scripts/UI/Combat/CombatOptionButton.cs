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

        [Header("Events to invoke or listen to")]
        [SerializeField]
        private SO_GenericEvent _CombatOptionChosenEvent;
        [SerializeField]
        private SO_GenericEvent _CombatSequenceEndedEvent;
        [SerializeField]
        private SO_GenericEvent _CombatSequenceRestartedEvent;
        [SerializeField]
        private SO_GenericEvent _CombatSequenceStartedEvent;

        private Button _Button;
        private Color _DisabledColor;
        private Color _NormalColor;

    #endregion

    #region Member Functions 

        private void Start()
        {
            // Listening to both start and restart events in case of a draw between players
            _CombatSequenceRestartedEvent.AddListener(OnCombatSequenceStarted);
            _CombatSequenceStartedEvent.AddListener(OnCombatSequenceStarted);

            _Button = GetComponent<Button>();
        }

        public void OnClick()
        {
            // Invoking the event with the player id and the option they have chosen
            LocalPlayerController localPlayerController = GameObject.FindObjectOfType<LocalPlayerController>();
            int[] data = { localPlayerController.GetComponent<PhotonView>().ViewID, (int) _CombatOptionToRepresent };
            _CombatOptionChosenEvent.Invoke(data);

            // De-highlighting the buttons after they have chosen their option
            DeHighlightButton();
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


