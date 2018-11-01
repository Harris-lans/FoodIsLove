using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using LLAPI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

public class HeroController : MonoBehaviour
{
	#region Member Variables

		[Header("Movement Details")]
		[SerializeField]
		private float _MinimumDistanceBeforeStoppingFromNode = 1.5f;

		[Space, Header("Inventory Details")]
		[SerializeField]
		private List<SO_UIMinionSlot> _IngredientInventorySlots;

		[Space, Header("Local Events for Tools")]
		[SerializeField]
		private UnityEvent _CollectedIngredientEvent;

		[Space, Header("Global Events")]
		[SerializeField]
		private SO_GenericEvent _HeroNearCookingStationEvent;
        [SerializeField]
        private SO_GenericEvent _HeroMovingAwayFromCookingStation;
		[SerializeField]
		private SO_GenericEvent _IngredientModifiedEvent;
		[SerializeField]
		private SO_GenericEvent _PickedupIngredientEvent;
		[SerializeField]
		private SO_GenericEvent _HeroSpawnedEvent;

		private SO_CombatData _CombatData;
		public bool IsLocal;
        public bool IsInCombat { get; private set; }
		private bool _CanCheckForCollision;
        public int OwnerID;
		private INavMover _Mover;
		private CookingStation _TargetCookingStation;
		private ANode _TargetNode;
		private GridSystem _GridSystem;
        private Coroutine _MovementCoroutine;

	#endregion

	#region Life Cycle

		private void Start()
		{
			_Mover = GetComponent<INavMover>();
			_GridSystem = GridSystem.Instance;
		    IsInCombat = false;
			_CanCheckForCollision = true;

			// Checking if the hero is local before changing the inventory slots
			if(IsLocal)
			{
				foreach (var slot in _IngredientInventorySlots)
				{
					// Initializing the MinionSlots
					slot.Initialize();
				}
			}
		}

		private void OnEnable() 
		{
			_CombatData = Resources.Load<SO_CombatData>("CombatData");
			_CombatData.CombatSequenceStartedEvent.AddListener(OnCombatSequenceStarted);
			_CombatData.CombatSequenceCompletedEvent.AddListener(OnCombatSequenceCompleted);
			PhotonNetwork.NetworkingClient.EventReceived += OnNetworkEvent;
		}

		private void OnDisable() 
		{
			_CombatData.CombatSequenceStartedEvent.RemoveListener(OnCombatSequenceStarted);
			_CombatData.CombatSequenceCompletedEvent.RemoveListener(OnCombatSequenceCompleted);
			PhotonNetwork.NetworkingClient.EventReceived -= OnNetworkEvent;
		}

		private void OnTriggerEnter(Collider other)
		{
            // Checking for hero collisions only in the master client
		    if (PhotonNetwork.IsMasterClient && !IsInCombat)
		    {
                // Checking if the heroes collided
		        HeroController hero = other.GetComponent<HeroController>();

		        if (hero != null)
		        {
					IsInCombat = true;
                    _CombatData.HeroesCollidedEvent.Invoke(null);
                    return;
		        }

				// Ignoring picking up of ingredients if they have entered a combat scenario
				IngredientMinion ingredient = other.GetComponent<IngredientMinion>();
				if (ingredient != null)
				{
					PickUpIngredient(ingredient);

					PhotonView ingredientView = ingredient.GetComponent<PhotonView>();
					Byterizer byterizer = new Byterizer();
					byterizer.Push(ingredientView.ViewID);
					byterizer.Push(GetComponent<PhotonView>().ViewID);
					byte[] data = byterizer.GetBuffer();

					// Initializing Network variables
					RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
					SendOptions sendOptions = new SendOptions { Reliability = true };
					PhotonNetwork.RaiseEvent((byte)NetworkedGameEvents.PICKED_UP_INGREDIENT, data, raiseEventOptions, sendOptions);
				}
		    }
		}

		private void OnTriggerExit(Collider other) 
		{
			// Checking if the heroes collided
			HeroController hero = other.GetComponent<HeroController>();

			if (hero != null)
			{
				_CanCheckForCollision = true;
			}
		}

        private IEnumerator MovingToNode(Vector3 positionToMoveTo)
        {
			_Mover.SetDestination(positionToMoveTo);

            while (Vector3.Distance(transform.position, _TargetNode.SpotToStandIn.position) >= _MinimumDistanceBeforeStoppingFromNode)
			{
				yield return null;
			}
			
			_TargetCookingStation = _TargetNode.GetComponent<CookingStation>();
			if (_TargetCookingStation != null)
			{
				// Invoking the near cooking station event only if the hero is controlled locally
				if (IsLocal)
				{
					_HeroNearCookingStationEvent.Invoke(_TargetCookingStation);
				}

				_TargetCookingStation.PickUpCookedFood(OwnerID);
			}

			//_Mover.StopMoving();
			_TargetNode = null;
        }

	#endregion

	#region Member Functions

		public void Initialize(bool isLocal)
		{
			IsLocal = isLocal;
			SpawnIdentificationRing();
		}

		private void SpawnIdentificationRing()
		{
			// Instantiating the identification ring
			PlayerIdentificationRing playerRingPrefab = Resources.Load<PlayerIdentificationRing>("PlayerIdentificationRing");
			PlayerIdentificationRing playerRing = Instantiate(playerRingPrefab, transform.position, Quaternion.identity);
			playerRing.transform.parent = transform;
			playerRing.Initialize(IsLocal);
		}

		public void MoveToNode(ANode nodeToMoveTo)
		{
			if (_TargetNode != null && !_Mover.ReachedDestination())
			{
				return;
			}

			if (IsLocal)
			{
				// The hero won't be able to cook now if he is moving away from the cooking station
				_HeroMovingAwayFromCookingStation.Invoke(null);
			}
			
			_TargetNode = nodeToMoveTo;

			if (_MovementCoroutine != null)
			{
				StopCoroutine(_MovementCoroutine);
			}

			_MovementCoroutine = StartCoroutine(MovingToNode(_TargetNode.SpotToStandIn.position));
		}

		public void PickUpIngredient(IngredientMinion ingredient)
		{
			for (int i = 0; i < _IngredientInventorySlots.Count; ++i)
			{

				if (_IngredientInventorySlots[i].Ingredient == null)
				{
				    if (IsLocal)
				    {
                        _IngredientInventorySlots[i].Ingredient = ingredient;

				        // Telling the UI that something has happened to some ingredient, so that they update themselves and play animations
				        _IngredientModifiedEvent.Invoke(null);
						_PickedupIngredientEvent.Invoke(_IngredientInventorySlots[i]);
				    }

					ingredient.OnPickedUp();
					_CollectedIngredientEvent.Invoke();
                    // Putting the ingredient in the backpack
				    ingredient.transform.parent = transform;
				    ingredient.transform.position = Vector3.zero;
				    ingredient.GetComponentInChildren<SpriteRenderer>().enabled = false;

                    return;
				}
			}
		}

		public void Cook(IngredientMinion ingredient, CookingPot cookingPot)
		{
            // Cooking the ingredient
			bool ingredientCooked = _TargetCookingStation.Use(ingredient);

            // Setting target cooking station to null
			_TargetCookingStation = null;

			// Updating ui and inventory only on the local machine
			if (IsLocal && ingredientCooked)
			{
				RemoveIngredientFromInventory(ingredient);

				// Telling the UI that something has happened to some ingredient, so that they update themselves
				_IngredientModifiedEvent.Invoke(null);
			}
		}

		private void RemoveIngredientFromInventory(IngredientMinion ingredient)
		{
			// Removing the ingredient from the slot
		    foreach (var inventorySlot in _IngredientInventorySlots)
		    {
		        if (inventorySlot.Ingredient == ingredient)
		        {
					PhotonNetwork.Destroy(inventorySlot.Ingredient.GetComponent<PhotonView>());
		            inventorySlot.Ingredient = null;
                    break;
		        }
		    }
		}

        private void OnCombatSequenceCompleted(object data)
        {
            IsInCombat = false;
			_Mover.ContinueMoving();
			_IngredientModifiedEvent.Invoke(null);
        }

        private void OnCombatSequenceStarted(object data)
        {
            IsInCombat = true;
			_Mover.StopMoving();
        }

		public void Kill()
		{
			if (!IsLocal)
			{
				return;
			}

			// Removing data from the inventory slots 
			for (int i = 0; i < _IngredientInventorySlots.Count; ++i)
			{
				if (_IngredientInventorySlots[i].Ingredient != null)
				{
					PhotonNetwork.Destroy(_IngredientInventorySlots[i].Ingredient.GetComponent<PhotonView>());
				}
				_IngredientInventorySlots[i].Ingredient = null;
			}

			// Telling the UI that something has happened to some ingredient, so that they update themselves
			_IngredientModifiedEvent.Invoke(null);

			PhotonNetwork.Destroy(GetComponent<PhotonView>());
		}

	#endregion

	#region Network Callbacks

		private void OnNetworkEvent(EventData eventData)
		{
			byte eventCode = eventData.Code;

			if (eventCode == (byte)NetworkedGameEvents.PICKED_UP_INGREDIENT)
			{
				Byterizer byterizer = new Byterizer();
				byterizer.LoadDeep((byte[])eventData.CustomData);

				int ingredientViewID = byterizer.PopInt32();
				int heroViewID = byterizer.PopInt32();

				if (heroViewID != GetComponent<PhotonView>().ViewID)
				{
					return;
				}

				PhotonView ingredientView = PhotonView.Find(ingredientViewID);
				IngredientMinion ingredientMinion = ingredientView.GetComponent<IngredientMinion>();
				if (ingredientMinion != null)
				{
					PickUpIngredient(ingredientMinion);
				}
			}
		}

	#endregion
}
