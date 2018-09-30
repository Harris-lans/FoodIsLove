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
				Debug.Log(screen.UIScreenTag);
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
			if (_CurrentScreen != null)
			{
				_CurrentScreen.HideScreen();
			}
			_CurrentScreen = _RegisteredScreens[screenTag];
			_CurrentScreen.ShowScreen();
		}
		
	#endregion
}
