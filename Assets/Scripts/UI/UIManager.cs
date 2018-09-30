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
	
	#endregion

	#region Life Cycle

		override protected void SingletonAwake()
		{
			_RegisteredScreens = new Dictionary<SO_Tag, UIScreen>();
			UIScreen[] screens = GetComponentsInChildren<UIScreen>(true);
			foreach(var screen in screens)
			{
				//_RegisteredScreens[screen.]
			}
		}

		override protected void SingletonStart () 
		{
			// Hiding all screens in case anyone is active
			foreach ( UIScreenType uiScreen in UIScreen.RegisteredScreens)
			{
				uiScreen.ScreenObject.HideScreen();
			}

			SetScreen(_DefaultScreen);	
		}

	#endregion

	#region Member Functions

		public void SetScreen(SO_Tag screenTag)
		{
			foreach ( UIScreenType uiScreen in UIScreen.RegisteredScreens)
			{
				if (uiScreen.ScreenTag == screenTag)
				{
					if (_CurrentScreen != null)
					{
						_CurrentScreen.HideScreen();
					}
					_CurrentScreen = uiScreen.ScreenObject;
					_CurrentScreen.ShowScreen();
					return;
				}
			}
		}
		
	#endregion
}
