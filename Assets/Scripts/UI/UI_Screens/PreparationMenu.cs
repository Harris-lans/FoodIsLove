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

        [Space, Header("Scroll Details")]
        [SerializeField]
        private float _ScrollVelocity = 10;
        [SerializeField]
        private float _ScrollStopDelta = 0.005f; 

        private int _IndexOfCurrentlySelectedCard;
        private int _NumberOfHeroCards;
        private LobbyManager _LobbyManager;

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
        }

        private void LateUpdate()
        {
            _IndexOfCurrentlySelectedCard = (int)Mathf.Floor(_CharacterSelectionScrollBar.value / (float)(1.0f / _NumberOfHeroCards)); 
            _IndexOfCurrentlySelectedCard = Mathf.Min(_IndexOfCurrentlySelectedCard, _NumberOfHeroCards - 1);

            // Updating the buttons
            _RightScrollButton.interactable = (_IndexOfCurrentlySelectedCard < _NumberOfHeroCards - 1);
            _LeftScrollButton.interactable = (_IndexOfCurrentlySelectedCard > 0);
        }

    #endregion

    #region Member Functions


        public void OnSelectReady()
        {
            // Passing the index of the hero to select
            _LobbyManager.ReadyUp(_IndexOfCurrentlySelectedCard);
            _UIManager.SetScreen(_ReadyScreenTag);
        }

        public void OnScrollRight()
        {
            if (_IndexOfCurrentlySelectedCard + 1 >= _NumberOfHeroCards)
            {
                return;
            }

            ++_IndexOfCurrentlySelectedCard;
            float nextTarget = (1.0f / _NumberOfHeroCards) * (float)(_IndexOfCurrentlySelectedCard + 1);
            StopAllCoroutines();
            StartCoroutine(ScrollRight(nextTarget));
        }

        public void OnScrollLeft()
        {
            if (_IndexOfCurrentlySelectedCard <= 0)
            {
                return;
            }

            --_IndexOfCurrentlySelectedCard;
            float nextTarget = (1.0f / _NumberOfHeroCards) * (float)(_IndexOfCurrentlySelectedCard);
            StopAllCoroutines();
            StartCoroutine(ScrollLeft(nextTarget));
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
            // Checking if the scroll should snap to the left or the right
            float expectedPosition = (1.0f / _NumberOfHeroCards) * (float)(_IndexOfCurrentlySelectedCard + 1);

            if (_CharacterSelectionScrollBar.value > expectedPosition)
            {

            }
        }

    #endregion
}
