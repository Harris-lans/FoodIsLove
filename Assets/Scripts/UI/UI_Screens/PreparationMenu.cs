using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PreparationMenu : UIScreen, IEndDragHandler
{
    #region Member Variables

        [Space, Header("UI Elements")]
        [SerializeField]
        private ScrollRect _HeroCardsScroller;
        [SerializeField]
        private Scrollbar _CharacterSelectionScrollBar;
        [SerializeField]
        private RectTransform _HeroCardContainers;
        [SerializeField]
        private Button _LeftScrollButton;
        [SerializeField]
        private Button _RightScrollButton;

        [Space, Header("Required Data")]
        [SerializeField]
        private SO_HeroList _HeroesList;

        [Space, Header("Screens to switch to")]
        [SerializeField]
        private SO_Tag _DishScreenTag;
        [SerializeField]
        private SO_Tag _ReadyScreenTag;
        [SerializeField]
        private SO_Tag _FoodWorldScreenTag;

        [Space, Header("Scroll Details")]
        [SerializeField]
        private float _ScrollVelocity = 10;
        [SerializeField]
        private float _ScrollStopDelta = 0.005f; 

        private int _IndexOfCurrentlySelectedCard;
        private int _NumberOfHeroCards;
        private LobbyManager _LobbyManager;
        private PhotonNetworkManager _PhotonNetworkManager;

    #endregion

    #region Life Cycle

        protected override void Awake() 
        {
            base.Awake();

            // Adding the hero cards to the hero selector
            foreach(var hero in _HeroesList.Heroes)
            {
                // Adding cards to the 
                Instantiate(hero.HeroCard, _HeroCardContainers);
            }
            _NumberOfHeroCards = _HeroesList.Heroes.Length;
            _IndexOfCurrentlySelectedCard = 0;
            _LobbyManager = LobbyManager.Instance;
            _PhotonNetworkManager = PhotonNetworkManager.Instance;
        }

        private void OnEnable()
        {
            _IndexOfCurrentlySelectedCard = 0;
            StartCoroutine(ScrollLeft(0));
            UpdateButtons();
        }

    #endregion

    #region Member Functions


        public void OnSelectReady()
        {
             _UIManager.SetScreen(_ReadyScreenTag);

            // Passing the index of the hero to select
            _LobbyManager.ReadyUp(_IndexOfCurrentlySelectedCard);
        }

        public void OnSelectCancel()
        {
            _PhotonNetworkManager.LeaveGame();
            _UIManager.SetScreen(_FoodWorldScreenTag);
        }

        private void UpdateButtons()
        {
            // Updating the buttons
            _RightScrollButton.interactable = (_IndexOfCurrentlySelectedCard < _NumberOfHeroCards - 1);
            _LeftScrollButton.interactable = (_IndexOfCurrentlySelectedCard > 0);
        }

        public void OnScrollRight()
        {
            if (_IndexOfCurrentlySelectedCard + 1 >= _NumberOfHeroCards)
            {
                return;
            }

            ++_IndexOfCurrentlySelectedCard;
            float nextTarget = (1.0f / (_NumberOfHeroCards - 1)) * (float)(_IndexOfCurrentlySelectedCard);
            StopAllCoroutines();
            StartCoroutine(ScrollRight(nextTarget));
            UpdateButtons();
        }

        public void OnScrollLeft()
        {
            if (_IndexOfCurrentlySelectedCard <= 0)
            {
                return;
            }

            --_IndexOfCurrentlySelectedCard;
            float nextTarget = (1.0f / (_NumberOfHeroCards - 1)) * (float)(_IndexOfCurrentlySelectedCard);
            StopAllCoroutines();
            StartCoroutine(ScrollLeft(nextTarget));
            UpdateButtons();
        } 

        private IEnumerator ScrollRight(float targetValue)
        {
            while(targetValue - _CharacterSelectionScrollBar.value >= 0.005f)
            {
                float step = Mathf.SmoothStep(_CharacterSelectionScrollBar.value, targetValue, _ScrollVelocity * Time.deltaTime);
                _CharacterSelectionScrollBar.value = step;
                yield return null;
            }
        }

        private IEnumerator ScrollLeft(float targetValue)
        {
            while (_CharacterSelectionScrollBar.value - targetValue >= 0.005f)
            {
                float step = Mathf.SmoothStep(_CharacterSelectionScrollBar.value, targetValue, _ScrollVelocity * Time.deltaTime);
                _CharacterSelectionScrollBar.value = step;
                yield return null;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("Moving right");
            if (eventData.delta.x > 0)
            {
                
            }
        }

    #endregion
}
