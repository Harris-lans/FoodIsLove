using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : UIScreen 
{
	#region Member Variables

        [SerializeField]
        private SO_MatchState _MatchState;

        [Space, Header("UI Elements")]
        [SerializeField]
        private Text _MatchResults;

        [Space, Header("Color")]
        [SerializeField]
        private Color _WinnerColor;
        [SerializeField]
        private Color _LoserColor;

    #endregion

    #region Life Cycle

        private void OnEnable() 
        {
            ShowMatchResults();
        }

    #endregion

    #region Member Functions

        private void ShowMatchResults()
        {
            if (_MatchState.WonTheMatch)
            {
                _MatchResults.color = _WinnerColor;
                _MatchResults.text = "You won";
            }
            else
            {
                _MatchResults.color = _LoserColor;
                _MatchResults.text = "You lost";
            }
        }

    #endregion
}
