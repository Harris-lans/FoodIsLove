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
        [SerializeField]
        private Text _MatchOverReason;
        [SerializeField]
        private Image _DishImage;

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

        private void OnDisable()
        {
            _DishImage.gameObject.SetActive(false);
        }

    #endregion

    #region Member Functions

        private void ShowMatchResults()
        {
            // Match is over because someone completed the dish
            if (_MatchState.GameOverReason == GameOverReason.DISH_COMPLETED_BY_SOMEONE)
            {
                _MatchResults.color = _LoserColor;
                _MatchResults.text = "You lost";
                _MatchOverReason.text = "Your opponent completed the dish";
                
                if (_MatchState.WonTheMatch)
                {
                    _MatchResults.color = _WinnerColor;
                    _MatchResults.text = "You won";
                    _MatchOverReason.text = "You completed the dish";
                    _DishImage.gameObject.SetActive(true);
                }

                return;
            }
            
            // Match is over because someone dropped out
            _MatchResults.color = _WinnerColor;
            _MatchResults.text = "You won the match. The other player bailed out";
        }

    #endregion
}

public enum GameOverReason : byte
{
    PLAYER_DROPPED,
    DISH_COMPLETED_BY_SOMEONE,
    GAME_NOT_OVER
}