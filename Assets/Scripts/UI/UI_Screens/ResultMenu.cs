using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultMenu : UIScreen
{
    #region Member Variables

        [Space, Header("Required Data")]
        [SerializeField]
        private SO_LobbyDetails _LobbyDetails;
        [SerializeField]
        private SO_MatchState _MatchState;
        [SerializeField]
        private Sprite _EmptyIcon;
        [SerializeField]
        private Sprite _WinnerIcon;
        [SerializeField]
        private Sprite _LoserIcon;

        [Space, Header("UI Elements")]
        [SerializeField]
        private Image _LocalPlayerImage;
        [SerializeField]
        private Image _RemotePlayerImage;
        [SerializeField]
        private Image _LocalPlayerWinnerImage;
        [SerializeField]
        private Image _RemotePlayerWinnerImage;

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
            if (_LobbyDetails.ChosenHero != null)
            {
                _LocalPlayerImage.sprite = _LobbyDetails.ChosenHero.Thumbnail;
            }
            if (_LobbyDetails.OpponentHero != null)
            {
                _RemotePlayerImage.sprite = _LobbyDetails.OpponentHero.Thumbnail;
            }
        }

        private void OnDisable()
        {
            _LocalPlayerWinnerImage.sprite = _EmptyIcon;
            _RemotePlayerWinnerImage.sprite = _EmptyIcon;
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
                    DeclareAsWinner();   
                }
                else
                {
                    DeclareAsLoser();
                }
            }
            else
            {
                DeclareAsWinner();
            }
        }

        private void DeclareAsWinner()
        {
            _LocalPlayerWinnerImage.sprite = _WinnerIcon;
            _RemotePlayerWinnerImage.sprite = _LoserIcon;
        }

        private void DeclareAsLoser()
        {
            _RemotePlayerWinnerImage.sprite = _WinnerIcon;
            _LocalPlayerWinnerImage.sprite = _LoserIcon;
        }

        private void OnClickNext()
        {
            SceneManager.LoadScene("MainMenu 1");
            _UIManager.SetScreen(_MainMenuScreenTag);
        }

    #endregion
}
