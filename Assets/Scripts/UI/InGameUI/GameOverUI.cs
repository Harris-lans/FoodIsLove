using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

        [Space, Header("Global Events")]
        [SerializeField]
        private SO_GenericEvent _LocalPlayerWonEvent;
        [SerializeField]
        private SO_GenericEvent _LocalPlayerLostEvent;

        [Space, Header("Color")]
        [SerializeField]
        private Color _WinnerColor;
        [SerializeField]
        private Color _LoserColor;

        [Space, Header("Screens to switch to")]
        [SerializeField]
        private SO_Tag _MainMenuScreenTag;

        [Space, Header("Screen Switching Details")]
        [SerializeField]
        private float _TimeBeforeGoingBackToTheMainMenu = 2.0f;

    #endregion

    #region Life Cycle

        private void OnEnable() 
        {
            ShowMatchResults();
            _DishImage.sprite = _MatchState.ExpectedDishes[0].DishThumbnail;
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
                if (_MatchState.WonTheMatch)
                {
                    _LocalPlayerWonEvent.Invoke(null);
                    _MatchResults.color = _WinnerColor;
                    _MatchResults.text = "You won";
                    _MatchOverReason.text = "You completed the dish";
                    _DishImage.gameObject.SetActive(true);
                }
                else
                {
                    _LocalPlayerLostEvent.Invoke(null);
                    _MatchResults.color = _LoserColor;
                    _MatchResults.text = "You lost";
                    _MatchOverReason.text = "Your opponent completed the dish";
                }
            }
            else
            {
                // Match is over because someone dropped out
                _MatchResults.color = _WinnerColor;
                _MatchResults.text = "You won";
                _MatchOverReason.text = "The other player dropped out";
            }

            // Going back to the main menu
            StartCoroutine(ReturnToMainMenu());
        }

        private IEnumerator ReturnToMainMenu()
        {
            yield return new WaitForSeconds(_TimeBeforeGoingBackToTheMainMenu);
            SceneManager.LoadScene("MainMenu 1");
            _UIManager.SetScreen(_MainMenuScreenTag);
        }

    #endregion
}

public enum GameOverReason : byte
{
    PLAYER_DROPPED,
    DISH_COMPLETED_BY_SOMEONE,
    GAME_NOT_OVER
}