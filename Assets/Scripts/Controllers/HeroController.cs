using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class HeroController : MonoBehaviour
{
	#region Member Variables

		[Header("Movement Details")]
		[SerializeField]
		private float _MinimumDistanceBeforeStoppingFromNode = 1.5f;
		[SerializeField]
		private float _RadiusForPickingUpTargetsOnReaching = 2.0f;

		[Space, Header("Inventory Details")]
		[SerializeField]
		private List<SO_UIMinionSlot> _IngredientInventorySlots;

		[Space, Header("Events")]
		[SerializeField]
		private SO_GenericEvent _HeroNearCookingStationEventHandler;
        [SerializeField]
        private SO_GenericEvent _HeroMovingAwayFromCookingStation;
		[SerializeField]
		private SO_GenericEvent _IngredientModifiedEvent;
	    
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

			foreach (var slot in _IngredientInventorySlots)
			{
				// Initializing the MinionSlots
				slot.Initialize();
			}
		}

		private void OnEnable() 
		{
			_CombatData = Resources.Load<SO_CombatData>("CombatData");
			_CombatData.CombatSequenceStartedEvent.AddListener(OnCombatSequenceStarted);
			_CombatData.CombatSequenceCompletedEvent.AddListener(OnCombatSequenceCompleted);
		}

		private void OnDisable() 
		{
			_CombatData.CombatSequenceStartedEvent.RemoveListener(OnCombatSequenceStarted);
			_CombatData.CombatSequenceCompletedEvent.RemoveListener(OnCombatSequenceCompleted);

			// Removing data from the inventory slots 
			for (int i = 0; i < _IngredientInventorySlots.Count; ++i)
			{

				if (IsLocal)
				{
					if (_IngredientInventorySlots[i].Ingredient != null)
					{
						Destroy(_IngredientInventorySlots[i].Ingredient);
					}

					_IngredientInventorySlots[i].Ingredient = null;

					// Telling the UI that something has happened to some ingredient, so that they update themselves
					_IngredientModifiedEvent.Invoke(null);
			    }
			}
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
					Debug.LogFormat("Hero collided with {0}", other.name);
					IsInCombat = true;
                    _CombatData.HeroesCollidedEvent.Invoke(null);
                    return;
		        }
		    }

            // Ignoring picking up of ingredients if they have entered a combat scenario
			IngredientMinion ingredient = other.GetComponent<IngredientMinion>();
			if (ingredient != null && !IsInCombat)
			{
				PickUpIngredient(ingredient);
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

        private IEnumerator MovingToNode(GridPosition cellToMoveTo)
        {
			_Mover.SetDestination(cellToMoveTo);

            while (Vector3.Distance(transform.position, _TargetNode.transform.position) > _MinimumDistanceBeforeStoppingFromNode)
			{
				yield return null;
			}
			
			_TargetCookingStation = _TargetNode.GetComponent<CookingStation>();
			if (_TargetCookingStation != null)
			{
				// Invokin the near cooking station event only if the hero is controlled locally
				if (IsLocal)
				{
					_HeroNearCookingStationEventHandler.Invoke(_TargetCookingStation);
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

			// Instantiating the identification ring
			PlayerIdentificationRing playerRingPrefab = Resources.Load<PlayerIdentificationRing>("PlayerIdentificationRing");
			PlayerIdentificationRing playerRing = Instantiate(playerRingPrefab, transform.position, Quaternion.identity);
			playerRing.transform.parent = transform;
			
			var netMatchDetails = Resources.Load<SO_NetMatchDetails>("NetMatchDetails");

			playerRing.Initialize(netMatchDetails.RemotePlayerColor);

			if (IsLocal)
			{
				playerRing.Initialize(netMatchDetails.LocalPlayerColor);
			}
		}

		public void MoveToNode(GridPosition cellToMoveTo, ANode nodeToMoveTo)
		{
			Debug.Log("Moving to node");
			if (_TargetNode != null)
			{
				return;
			}

			// The hero won't be able to cook now if he is moving away from the cooking station
		    _HeroMovingAwayFromCookingStation.Invoke(null);
			
			_TargetNode = nodeToMoveTo;

			if (_MovementCoroutine != null)
			{
				StopCoroutine(_MovementCoroutine);
			}

			_MovementCoroutine = StartCoroutine(MovingToNode(cellToMoveTo));
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

				        // Telling the UI that something has happened to some ingredient, so that they update themselves
				        _IngredientModifiedEvent.Invoke(null);
				    }

					ingredient.OnPickedUp();
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
		            inventorySlot.Ingredient = null;
                    break;
		        }
		    }
		}

        private void OnCombatSequenceCompleted(object data)
        {
            IsInCombat = false;
			_Mover.ContinueMoving();
        }

        private void OnCombatSequenceStarted(object data)
        {
            IsInCombat = true;
			_Mover.StopMoving();
        }

		public void Kill()
		{
			Debug.Log("Lost in combat, doofus");
			PhotonNetwork.Destroy(GetComponent<PhotonView>());
		}

	#endregion
}
