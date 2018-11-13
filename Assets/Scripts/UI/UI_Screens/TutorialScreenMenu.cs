using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScreenMenu : UIScreen
{
	#region Member Variables

        [Space, Header("UI Elements")]
        [SerializeField]
        private Scrollbar _TutorialScreenScrollBar;
        [SerializeField]
        private Button _LeftScrollButton;
        [SerializeField]
        private Button _RightScrollButton;
        [SerializeField]
        private Button _ExitButton;

        [Space, Header("Tutorial Details")]
        [SerializeField]
        private int _NumberOfTutorialPages;

        [Space, Header("Screens to switch to")]
        [SerializeField]
        private SO_Tag _FoodWorldScreenTag;

        [Space, Header("Scroll Details")]
        [SerializeField]
        private float _ScrollVelocity = 10;
        [SerializeField]
        private float _ScrollStopDelta = 0.005f; 

        private int _IndexOfCurrentlySelectedTutorial;

    #endregion

	#region Life Cycle

		private void OnEnable()
        {
            _IndexOfCurrentlySelectedTutorial = 0;
            _TutorialScreenScrollBar.value = 0;
            UpdateButtons();
        }

	#endregion

	#region Member Functions

		public void OnSelectCancel()
        {
            _UIManager.SetScreen(_FoodWorldScreenTag);
        }

        private void UpdateButtons()
        {
            // Updating the buttons
            _RightScrollButton.interactable = (_IndexOfCurrentlySelectedTutorial < _NumberOfTutorialPages - 1);
            _LeftScrollButton.interactable = (_IndexOfCurrentlySelectedTutorial > 0);
        }

		public void OnScrollRight()
        {
            if (_IndexOfCurrentlySelectedTutorial + 1 >= _NumberOfTutorialPages)
            {
                return;
            }

            ++_IndexOfCurrentlySelectedTutorial;
            float nextTarget = (1.0f / (_NumberOfTutorialPages - 1)) * (float)(_IndexOfCurrentlySelectedTutorial);
            StopAllCoroutines();
            StartCoroutine(ScrollRight(nextTarget));
            UpdateButtons();
        }

        public void OnScrollLeft()
        {
            if (_IndexOfCurrentlySelectedTutorial <= 0)
            {
                return;
            }

            --_IndexOfCurrentlySelectedTutorial;
            float nextTarget = (1.0f / (_NumberOfTutorialPages - 1)) * (float)(_IndexOfCurrentlySelectedTutorial);
            StopAllCoroutines();
            StartCoroutine(ScrollLeft(nextTarget));
            UpdateButtons();
        } 

        private IEnumerator ScrollRight(float targetValue)
        {
            while(targetValue - _TutorialScreenScrollBar.value >= 0.005f)
            {
                float step = Mathf.SmoothStep(_TutorialScreenScrollBar.value, targetValue, _ScrollVelocity * Time.deltaTime);
                _TutorialScreenScrollBar.value = step;
                yield return null;
            }
        }

        private IEnumerator ScrollLeft(float targetValue)
        {
            while (_TutorialScreenScrollBar.value - targetValue >= 0.005f)
            {
                float step = Mathf.SmoothStep(_TutorialScreenScrollBar.value, targetValue, _ScrollVelocity * Time.deltaTime);
                _TutorialScreenScrollBar.value = step;
                yield return null;
            }
        }

	#endregion
}
