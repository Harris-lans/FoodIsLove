using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : SingletonBehaviour<UIManager> 
{
	#region  Member Variables

		[SerializeField]
		private SO_Tag _DefaultScreen;

		private UIScreen _CurrentScreen;
		private Dictionary<SO_Tag, UIScreen> _RegisteredScreens;
        private bool _OutroAnimationsComplete;
		private GraphicRaycaster _GraphicRaycaster;
	
	#endregion

	#region Life Cycle

		override protected void SingletonAwake()
		{
			_RegisteredScreens = new Dictionary<SO_Tag, UIScreen>();
			UIScreen[] screens = GetComponentsInChildren<UIScreen>(true);
		    _OutroAnimationsComplete = true;
			_GraphicRaycaster = GetComponent<GraphicRaycaster>();
			_GraphicRaycaster.enabled = false;

			foreach(var screen in screens)
			{
				_RegisteredScreens[screen.UIScreenTag] = screen;
			}
		}

		override protected void SingletonStart () 
		{
			SetScreen(_DefaultScreen);
		}

	#endregion

	#region Member Functions

		public void SetScreen(SO_Tag screenTag)
		{
			// In case the current screen is animating
			if (_CurrentScreen != null && _CurrentScreen.IsAnimating)
			{
				StopAllCoroutines();
				_CurrentScreen.HideScreen();
			}
		    StartCoroutine(StartScreenTransition(screenTag));
		}

        public IEnumerator StartScreenTransition(SO_Tag nextScreenTag)
        {
			_GraphicRaycaster.enabled = false;			

            if (_CurrentScreen != null)
            { 
                yield return StartCoroutine(_CurrentScreen.PlayOutroAnimations());
            }

			if (_RegisteredScreens.ContainsKey(nextScreenTag))
			{
				var nextScreen = _RegisteredScreens[nextScreenTag];
				StartCoroutine(nextScreen.PlayIntroAnimations());
			
				while(nextScreen.State != UIScreenState.VISIBLE)
				{
					yield return null;
				}
				_CurrentScreen = nextScreen;
			}
			else
			{
				_CurrentScreen = null;
			}

			_GraphicRaycaster.enabled = true;
        }

    #endregion

	#region Properties

		public UIScreen CurrentScreen
		{
			get 
			{
				return _CurrentScreen;
			}
		}

	#endregion
}
