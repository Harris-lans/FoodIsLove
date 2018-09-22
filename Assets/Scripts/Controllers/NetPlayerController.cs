using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using LLAPI;
using Photon.Pun;
using UnityEngine;

public class NetPlayerController : APlayerController 
{
	#region Member Variables

		

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

        public override void Initialize(HeroController hero)
        {
            base.Initialize(hero);
            hero.IsLocal = false;
        }

        #region Network Callbacks

            private void OnNetworkEvent(EventData eventData)
            {
                byte eventCode = eventData.Code;

                // On Node Selected
                if (eventCode == (byte)NetworkedGameEvents.ON_SELECTED_NODE)
                {
                    // Extracting data
					byte[] data = (byte[]) eventData.CustomData; 
					Byterizer byterizer = new Byterizer();
                    byterizer.LoadDeep(data);

                    int playerControllerViewID = byterizer.PopInt32();

					// Checking if the event is for this player controller
					if (_PhotonView.ViewID != playerControllerViewID)
					{
						// Event not meant for this player controller
						return;
					}

                    // Loading the left over data
                    int nodeViewID = byterizer.PopInt32();
                    GridPosition selectedCell = new GridPosition(byterizer.PopByte(), byterizer.PopByte());
                    ANode node = PhotonView.Find(nodeViewID).GetComponent<ANode>();

                    OnSelectedNode(selectedCell, node);
                }

                // On Ingredient selected
                else if (eventCode == (byte)NetworkedGameEvents.ON_SELECTED_INGREDIENT)
                {
                    // Extracting data
					byte[] data = (byte[]) eventData.CustomData; 
					Byterizer byterizer = new Byterizer();
                    byterizer.LoadDeep(data);

                    int playerControllerViewID = byterizer.PopInt32();

					// Checking if the event is for this player controller
					if (_PhotonView.ViewID != playerControllerViewID)
					{
						// Event not meant for this player controller
						return;
					}

                    // Loading the left over data
                    int ingredientViewID = byterizer.PopInt32();
                    IngredientMinion ingredient = PhotonView.Find(ingredientViewID).GetComponent<IngredientMinion>(); 
                    OnSelectedIngredient(ingredient);
                }
            }

        #endregion

    #endregion
}
