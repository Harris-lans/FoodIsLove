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

        [Space, Header("UI Elements")]
        [SerializeField]
        private Image _LocalPlayerImage;
        [SerializeField]
        private Image _RemotePlayerImage;
        [SerializeField]
        private Image _LocalPlayerWinner;
        [SerializeField]
        private Image _RemotePlayerWinner;

        [Space, Header("Global Events")]
        [SerializeField]
        private SO_GenericEvent _LocalPlayerWonEvent;
        [SerializeField]
        private SO_GenericEvent _LocalPlayerLostEvent;


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
            _LocalPlayerImage.sprite = _LobbyDetails.ChosenHero.Thumbnail;
            _RemotePlayerImage.sprite = _LobbyDetails.OpponentHero.Thumbnail;
        }

        private void OnDisable()
        {
            _LocalPlayerWinner.sprite = _EmptyIcon;
            _RemotePlayerWinner.sprite = _EmptyIcon;
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
            _LocalPlayerWonEvent.Invoke(null);
            _LocalPlayerWinner.sprite = _WinnerIcon;
        }

        private void DeclareAsLoser()
        {
            _LocalPlayerLostEvent.Invoke(null);
            _RemotePlayerWinner.sprite = _WinnerIcon;
        }

        private void OnClickNext()
        {
            SceneManager.LoadScene("MainMenu 1");
            _UIManager.SetScreen(_MainMenuScreenTag);
        }

    #endregion
}
