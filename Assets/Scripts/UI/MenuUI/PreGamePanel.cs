using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreGamePanel : UIScreen 
{
	#region Member Variables
	
		[Header("Text Labels")]
		[SerializeField]
		private Text _PlayersConnectedText;
		[SerializeField]
		private Text _PlayersReadyText;

		[Space, Header("Lobby Details")]
		[SerializeField]
		private SO_LobbyDetails _LobbyDetails;
	
	#endregion

	#region Life Cycle

		private void LateUpdate()
		{
			_PlayersConnectedText.text = "Number of Players Connected     : " + _LobbyDetails.NumberOfPlayersInRoom;
			_PlayersReadyText.text     = "Number of Players Ready             : " + _LobbyDetails.NumberOfPlayersReady;
		}

	#endregion
}
