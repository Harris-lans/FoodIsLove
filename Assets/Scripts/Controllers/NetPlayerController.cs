using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using LLAPI;
using Photon.Pun;
using UnityEngine;

public class NetPlayerController : APlayerController 
{
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
            hero.Initialize(false);
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
                    ANode node = PhotonView.Find(nodeViewID).GetComponent<ANode>();

                    OnSelectedNode(node);
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

                // On heroes collided
                else if (eventCode == (byte) NetworkedGameEvents.ON_HEROES_COLLIDED_EVENT)
                {
                    if(PhotonNetwork.IsMasterClient)
                    {
                        return;
                    }   
                    _CombatData.HeroesCollidedEvent.Invoke(null);
                }

                else if (eventCode == (byte) NetworkedGameEvents.ON_COMBAT_SEQUENCE_RESTARTED)
                {   
                    if(PhotonNetwork.IsMasterClient)
                    {
                        return;
                    }   
                    _CombatData.CombatSequenceRestartedEvent.Invoke(null);
                }

                // On combat option chosen
                else if (eventCode == (byte) NetworkedGameEvents.ON_SELECTED_COMBAT_OPTION)
                {
                    /*  Combat validation only takes place in master client so the others need not be
                        updated when someone choses. Its enough if they see the results */
                    if (!PhotonNetwork.IsMasterClient)
                    {
                        return;
                    }

                    byte[] data = (byte[])eventData.CustomData;
                    Byterizer byterizer = new Byterizer();
                    byterizer.LoadDeep(data);

                    int playerViewID = byterizer.PopInt32();
                    byte chosenOption = (byte)byterizer.PopByte();
                    int[] combatData = {playerViewID, chosenOption};

                    _CombatData.CombatOptionChosenEvent.Invoke(combatData);
                }

                // On show combat results event
                else if (eventCode == (byte) NetworkedGameEvents.ON_COMBAT_SEQUENCE_RESULT)
                {
                    // This event is already triggered in the master client so it needs to be triggered only on other clients
                    if (PhotonNetwork.IsMasterClient)
                    {
                        return;
                    }

                    byte[] data = (byte[])eventData.CustomData;
                    Byterizer byterizer = new Byterizer();
                    byterizer.LoadDeep(data);

                    int playerViewID = byterizer.PopInt32();

                    byte chosenOption = (byte)byterizer.PopByte();
                    int winnerID = byterizer.PopInt32();
                    int[] combatData = {playerViewID, chosenOption, winnerID};
                    _CombatData.ShowCombatResultsEvent.Invoke(combatData);
                }

                // On combat sequence ended event
                else if (eventCode == (byte) NetworkedGameEvents.ON_COMBAT_SEQUENCE_ENDED)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        return;
                    }

                    _CombatData.CombatSequenceCompletedEvent.Invoke((int)eventData.CustomData);
                }
            }

        #endregion

    #endregion
}
