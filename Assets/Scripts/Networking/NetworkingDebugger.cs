using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkingDebugger : SingletonBehaviour<NetworkingDebugger> 
{
	#region Member Functions

		[SerializeField]
		private bool _Debug = true;

	#endregion

	#region Life Cycle

		private void OnGUI() 
		{
			if (_Debug)
			{
				GUILayout.BeginHorizontal();

					GUILayout.BeginVertical();

						GUI.color = Color.green;
						GUILayout.Label("Network Debugger: ");
						GUILayout.Space(3);
						GUI.color = Color.white;
						GUILayout.Label("Connection Status " + PhotonNetwork.ConnectMethod.ToString());
						GUILayout.Label("Server: " + PhotonNetwork.CloudRegion);
						GUILayout.Label("Ping: " + PhotonNetwork.GetPing());
						GUILayout.Label("In Room: " + PhotonNetwork.InRoom);
						if (PhotonNetwork.InRoom)
						{
							GUILayout.Label("Room name: " + PhotonNetwork.CurrentRoom.Name);
							GUILayout.Label("Maximum players allowed in room: " + PhotonNetwork.CurrentRoom.MaxPlayers);
							GUILayout.Label("Number of players in room:" + PhotonNetwork.CurrentRoom.PlayerCount);
						}

					GUILayout.EndVertical();

				GUILayout.EndHorizontal();
			}	
		}

        private void Start()
        {
            //AkSoundEngine.PostEvent("UI_IG_Select", gameObject);
        }

	#endregion

}