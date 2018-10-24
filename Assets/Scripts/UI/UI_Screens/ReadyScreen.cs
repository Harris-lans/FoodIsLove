using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyScreen : UIScreen 
{
	#region Member Variables

		[Space, Header("Required Data")]
		[SerializeField]
		private SO_LobbyDetails _LobbyDetails;

		[Space, Header("UI Elements")]
		[SerializeField]
		private Image _LocalPlayerImage;
		[SerializeField]
		private Image _RemotePlayerImage;
		[SerializeField]
		private Text _LocalPlayerStatusText;
		[SerializeField]
		private Text _RemotePlayerStatusText;

		[Space, Header("Sprite Data")]
		[SerializeField]
		private Sprite _EmptyIcon;

		[Space, Header("Screens to switch to")]
		[SerializeField]
		private SO_Tag _PreparationScreen;

		private LobbyManager _LobbyManager;

	#endregion

	#region Life Cycle

		protected override void Awake()
		{
			base.Awake();
		} 

		private void OnEnable()
		{
			UpdateUI();
			_LobbyManager = LobbyManager.Instance;
			_LobbyManager.OnPlayerStatusChange.AddListener(OnPlayerStatusChange);
		}

		private void OnDisable() 
		{
			_LobbyManager.OnPlayerStatusChange.RemoveListener(OnPlayerStatusChange);
		}

	#endregion

	#region Member Functions

		public void OnCancel()
		{
			_UIManager.SetScreen(_PreparationScreen);
			_LobbyManager.NotReady();
		}

		private void UpdateUI()
		{
			if (_LobbyDetails.ChosenHero == null)
			{
				_LocalPlayerImage.sprite = _EmptyIcon;
				_LocalPlayerStatusText.text = "Not Ready";
				_LocalPlayerStatusText.color = Color.red;
			}
			else
			{
				_LocalPlayerImage.sprite = _LobbyDetails.ChosenHero.Thumbnail;
				_LocalPlayerStatusText.text = "Ready";
				_LocalPlayerStatusText.color = Color.green;
			}

			if (_LobbyDetails.OpponentHero == null)
			{
				_RemotePlayerImage.sprite = _EmptyIcon;
				_RemotePlayerStatusText.text = "Not Ready";
				_RemotePlayerStatusText.color = Color.red;
			}

			else
			{
				_RemotePlayerImage.sprite = _LobbyDetails.OpponentHero.Thumbnail;
				_RemotePlayerStatusText.text = "Ready";
				_RemotePlayerStatusText.color = Color.green;
			}
		}

		private void OnPlayerStatusChange()
		{
			UpdateUI();
		}

	#endregion
}
