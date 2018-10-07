using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : UIScreen 
{
	#region Member Variables

        [SerializeField]
        private SO_MatchState _MatchState;

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

        }

    #endregion
}
