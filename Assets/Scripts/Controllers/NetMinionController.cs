using ExitGames.Client.Photon;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetMinionController : MinionController 
{
    #region Member Variables

        // Nothing to add here for now

    #endregion

    #region Life Cycle

        private void OnEnable()
        {
            PhotonNetwork.NetworkingClient.EventReceived += OnNetworkEvent;
        }

        private void OnDisable()
        {
            PhotonNetwork.NetworkingClient.EventReceived -= OnNetworkEvent;
        }

	#endregion

	#region Member Functions

        private void MoveToPosition(Vector3 targetPosition)
        {
            // Interpolating between the target and the current position
            _NavMeshAgent.SetDestination(targetPosition);
        }

        #region Network Callbacks

            private void OnNetworkEvent(EventData eventData)
			{
                NetworkedGameEvents eventType = (NetworkedGameEvents)eventData.Code;
                
				// On receiving movement instructions
                if (eventType == NetworkedGameEvents.ON_MINION_MOVED)
                {
                    float[] data = (float[])eventData.CustomData;
                    if ((int)data[0] != _PhotonView.ViewID)
                    {
                        return;
                    }

					/*
						Using nav mesh agent in a controlled way to get smoother movement and accurate replication.
						As giving only one target to a navmesh agents can cause the navmesh agents to switch positions
						around the target location
					*/

                    Vector3 targetPosition = new Vector3(data[1], transform.position.y, data[2]);
                    MoveToPosition(targetPosition);
                }

				// On receiving cook instructions
                else if (eventType == NetworkedGameEvents.ON_MINION_COOK)
                {
                    int[] data = (int[])eventData.CustomData;
					if (data[0] != _PhotonView.ViewID)
					{
						return;
					}
					PhotonView cookingStationView = PhotonView.Find(data[1]);
					if (cookingStationView != null)
					{
						CookingStation cookingStation = cookingStationView.GetComponent<CookingStation>();
						
						if (cookingStation != null)
						{
                            Debug.Log("Cooking replicated");
							Cook(cookingStation);
						}
					}
                }
            }

        #endregion

    #endregion
}
