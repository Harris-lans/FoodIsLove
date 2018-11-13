using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaveMatchScreen : UIScreen 
{
	#region Member Variables

		[SerializeField]
		private SO_Tag _GameScreenTag;
		[SerializeField]
		private SO_Tag _MainMenuScreenTag;

		private PhotonNetworkManager _PhotonNetworkManager;

	#endregion

	#region Life Cycle

		private void Start()
		{
			_PhotonNetworkManager = PhotonNetworkManager.Instance;
		}

	#endregion

	#region Member Functions

		public void OnPressedQuit()
		{
			_PhotonNetworkManager.LeaveGame();
			SceneManager.LoadScene("MainMenu 1");
			_UIManager.SetScreen(_MainMenuScreenTag);
		}

		public void OnPressedCancel()
		{
			_UIManager.SetScreen(_GameScreenTag);
		}

	#endregion
}
