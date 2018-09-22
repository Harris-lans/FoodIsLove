using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinGameScreen : UIScreen 
{
	#region Member Variables

		[Space, Header("Input Fields for Network Configuration")]
		[SerializeField]
		private InputField _JoinGameTextField;
		[SerializeField]
		private Button _CreateRoomButton; 
		
		[Header("Screen Transition Details: ")]
		[SerializeField]
		private SO_Tag _ScreenToChangeToOnJoinGame;

		private PhotonNetworkManager _PhotonNetworkManager;

	#endregion

	#region Life Cycle

		override protected void Start()
		{
			base.Start();
			_PhotonNetworkManager = PhotonNetworkManager.Instance;
		}

	#endregion

	#region Member Functions

		public void JoinGame()
		{
			_PhotonNetworkManager.CreateOrJoinGame(_JoinGameTextField.text);
			_UIManager.SetScreen(_ScreenToChangeToOnJoinGame);
		}

	#endregion
}
