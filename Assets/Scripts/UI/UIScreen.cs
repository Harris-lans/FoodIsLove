using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScreen : MonoBehaviour 
{
	#region Static Variables

		public static List<UIScreenType> RegisteredScreens;

	#endregion

	#region Member Variables
		
		[Header("Screen Details")]
		[SerializeField]
		public SO_Tag _UIScreenTag;

		[Header("Animations")]
		public Animation[] AnimatonsToPlayOnTransitioningIn;
		protected Animation[] AnimatonsToPlayOnTransitioningOut;

		protected UIManager _UIManager;

		private UIScreenType _ScreenType;

	#endregion

	#region Life Cycle

		protected virtual void Awake()
		{
			RegisterScreen();
			gameObject.SetActive(false);
		}

		protected virtual void Start()
		{
			_UIManager = UIManager.Instance;
		}

		private void OnDestroy()
		{
			UnregisterScreen();
		}

	#endregion

	#region Member Functions

		public void ShowScreen()
		{
			gameObject.SetActive(true);
		}

		public void HideScreen()
		{
			gameObject.SetActive(false);
		}

		private void RegisterScreen()
		{
			if (_UIScreenTag != null)
			{
				UIScreenType screenType = new UIScreenType 
				{ 
					ScreenTag = _UIScreenTag,
					ScreenObject = this
				};
				RegisteredScreens.Add(screenType);
				_ScreenType = screenType;
			}
		}

		private void UnregisterScreen()
		{
			if (RegisteredScreens.Contains(_ScreenType))
			{
				RegisteredScreens.Remove(_ScreenType);
			}
		}

	#endregion
}

[System.Serializable]
public class UIScreenType
{
	public SO_Tag ScreenTag;
	public UIScreen ScreenObject;
}