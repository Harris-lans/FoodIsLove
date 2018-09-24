using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Photon.Pun;
using UnityEngine;

public class CombatManager : MonoBehaviour 
{
    #region Member Variables

    [Header("Events to listen to or invoke")]
    [SerializeField]
    private SO_GenericEvent _CombatSequenceStartedEvent;
    [SerializeField]
    private SO_GenericEvent _CombatOptionChosenEvent;
    [SerializeField]
    private SO_GenericEvent _CombatSequenceCompletedEvent;
    [SerializeField]
    private SO_GenericEvent _CombatSequenceRestartedEvent;

    private Dictionary<int, CombatOptionButton.CombatOptions> _PlayersAndTheirCombatOption;
    private Coroutine _CombatResolver;

    [Header("Combat Rules")]
    [SerializeField]
    private CombatRules[] _CombatRules;


    #endregion

    #region Life Cycle

        private void Awake()
        {
            // Combat manager is run only in the master client
            if (!PhotonNetwork.IsMasterClient)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        { 
            _CombatSequenceStartedEvent.AddListener(OnCombatSequenceStarted);
        }

    #endregion

    #region Member Functions

        private void OnCombatSequenceStarted(object data)
        {
            // Starting to resolve combat also waiting for all
            StartCoroutine(ResolveCombat());
        }

        private IEnumerator ResolveCombat()
        {
            _PlayersAndTheirCombatOption = new Dictionary<int, CombatOptionButton.CombatOptions>();
            bool combatResolved = false;

            while (!combatResolved)
            {
                if (_PlayersAndTheirCombatOption.Count >= PhotonNetwork.CountOfPlayersInRooms)
                {
                    int winner = ValidateWinner(_PlayersAndTheirCombatOption);

                    // It was a draw
                    if (winner == 0)
                    {
                        _CombatSequenceRestartedEvent.Invoke(null);
                        continue;
                    }
                }
                yield return null;
            }
        }

        private int ValidateWinner(Dictionary<int, CombatOptionButton.CombatOptions> playerCombatChoices)
        {
            KeyValuePair<int, CombatOptionButton.CombatOptions> player1CombatChoice = playerCombatChoices.ElementAt(0);
            KeyValuePair<int, CombatOptionButton.CombatOptions> player2CombatChoice = playerCombatChoices.ElementAt(1);

            foreach (var rule in _CombatRules)
            {
                // Player - 1 won
                if (rule.CombatOption == player1CombatChoice.Value && rule.CombatOption == player2CombatChoice.Value)
                {
                    return player1CombatChoice.Key;
                }

                // Player - 2 won
                else if (rule.CombatOption == player2CombatChoice.Value && rule.CombatOption == player1CombatChoice.Value)
                {
                    return player2CombatChoice.Key;
                }
            }
            
            // It was a draw
            return 0;
        }

        private void OnCombatOptionChosen(object data)
        {
            int[] combatData = (int[]) data;

            _PlayersAndTheirCombatOption[combatData[0]] = (CombatOptionButton.CombatOptions)combatData[1];
        }

    #endregion

    public struct CombatRules
    {
        public CombatOptionButton.CombatOptions CombatOption;
        public CombatOptionButton.CombatOptions Defeats;
    }
}
