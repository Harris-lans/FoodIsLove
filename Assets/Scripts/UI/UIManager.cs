using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class UIManager : SingletonBehaviour<UIManager> 
{
	#region  Member Variables

		[SerializeField]
		private SO_Tag _DefaultScreen;

		private UIScreen _CurrentScreen;
		private Dictionary<SO_Tag, UIScreen> _RegisteredScreens;
        private bool _OutroAnimationsComplete;
	
	#endregion

	#region Life Cycle

		override protected void SingletonAwake()
		{
			_RegisteredScreens = new Dictionary<SO_Tag, UIScreen>();
			UIScreen[] screens = GetComponentsInChildren<UIScreen>(true);
		    _OutroAnimationsComplete = true;

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
		    StartCoroutine(StartScreenTransition(_CurrentScreen, _RegisteredScreens[screenTag]));
		    _CurrentScreen = _RegisteredScreens[screenTag];
		}

        public IEnumerator StartScreenTransition(UIScreen currentScreen, UIScreen nextScreen)
        {
            if (currentScreen != null)
            { 
                StartCoroutine(currentScreen.PlayOutroAnimations());
                
                // Waiting for the current screen to transition out
                while (currentScreen.State != UIScreenState.HIDDEN)
                {
                    yield return null;
                }
            }

            StartCoroutine(nextScreen.PlayIntroAnimations());
        }

    #endregion
}
