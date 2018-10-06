using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Photon.Pun;
using UnityEngine;

public class CombatManager : MonoBehaviour 
{
    #region Member Variables

    [Header("Combat Information")]
    [SerializeField]
    private SO_CombatData _CombatData;
    [SerializeField]
    private float _TimeToWaitBeforeClosingThePoll = 4;
    [SerializeField]
    private float _TimeBeforeCompletingCombatAfterThePoll = 3;

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
            _CombatData.CombatSequenceStartedEvent.AddListener(OnCombatSequenceStarted);
            _CombatData.CombatOptionChosenEvent.AddListener(OnCombatOptionChosen);
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
                Debug.LogFormat("Number of players who have polled: {0}", _PlayersAndTheirCombatOption.Count);
                
                // Waiting for both the players to poll in their option
                if (_PlayersAndTheirCombatOption.Count >= 2)
                {
                    // Determining the winner
                    int winner = ValidateWinner(_PlayersAndTheirCombatOption);
                    
                    foreach(var player in _PlayersAndTheirCombatOption )
                    {
                        int[] data = {player.Key, (int)player.Value};

                        // Invoking the show results event
                        _CombatData.ShowCombatResultsEvent.Invoke(data);
                    }

                    combatResolved = true;
                    StartCoroutine(WaitBeforeFinishingCombat(winner));
                }
                yield return null;
            }
        }

        private IEnumerator WaitBeforeFinishingCombat(int winner)
        {
            Debug.Log("Completing combat in some time");
            yield return new WaitForSeconds(_TimeBeforeCompletingCombatAfterThePoll);

            // It was a draw
            if (winner == 0)
            {
                //FIXME: This has to be fixed. Ignoring draw condition for M2
                // Re-starting the poll and telling the other client 
                _CombatData.CombatSequenceStartedEvent.Invoke(null);
            }
            else
            {
                _CombatData.CombatSequenceCompletedEvent.Invoke(winner);
            }
        } 

        private int ValidateWinner(Dictionary<int, CombatOptionButton.CombatOptions> playerCombatChoices)
        {
            KeyValuePair<int, CombatOptionButton.CombatOptions> player1CombatChoice = playerCombatChoices.ElementAt(0);
            KeyValuePair<int, CombatOptionButton.CombatOptions> player2CombatChoice = playerCombatChoices.ElementAt(1);

            foreach (var rule in _CombatRules)
            {
                // Player - 1 won
                if (rule.CombatOption == player1CombatChoice.Value && rule.Defeats == player2CombatChoice.Value)
                {
                    return player1CombatChoice.Key;
                }

                // Player - 2 won
                else if (rule.CombatOption == player2CombatChoice.Value && rule.Defeats == player1CombatChoice.Value)
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
            // Recieving events from the Combat UI and the NetPlayerController to recieve options from both the players
            _PlayersAndTheirCombatOption[combatData[0]] = (CombatOptionButton.CombatOptions)combatData[1];
        }

    #endregion

    [System.Serializable]
    public struct CombatRules
    {
        public CombatOptionButton.CombatOptions CombatOption;
        public CombatOptionButton.CombatOptions Defeats;
    }
}
