using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HeroController : MonoBehaviour 
{
	#region Member Variables

		[Header("Movement Details")]
		[SerializeField]
		private float _MinimumDistanceBeforeInteractingWithStation = 1.5f;
		[SerializeField]
		private float _RadiusForPickingUpTargetsOnReaching = 2.0f;

		[Space, Header("Inventory Details")]
		[SerializeField]
		private List<SO_UIMinionSlot> _IngredientInventorySlots;

		[Space, Header("Events")]
		public UnityEvent PickedUpIngredientEvent;
		[SerializeField]
		private SO_GenericEvent _HeroNearCookingStationEventHandler;

		public bool IsLocal;
		private INavMover _Mover;
		private CookingStation _TargetCookingStation;
		private GridSystem _GridSystem;
		[HideInInspector]
	
	#endregion

	#region Life Cycle

		private void Start()
		{
			_Mover = GetComponent<INavMover>();
			_GridSystem = GridSystem.Instance;
			foreach (var slot in _IngredientInventorySlots)
			{
				// Initializing the MinionSlots
				slot.Initialize();
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			IngredientMinion ingredient = other.GetComponent<IngredientMinion>();
			if (ingredient != null)
			{
				PickUpIngredient(ingredient);
			}
		}

		private void Update()
		{
			if (_TargetCookingStation == null)
			{
				return;
			}

			if (Vector3.Distance(transform.position, _TargetCookingStation.transform.position) <= _MinimumDistanceBeforeInteractingWithStation)
			{
				Debug.Log("Hero Near Cooking Station");
				_Mover.StopMoving();
				_HeroNearCookingStationEventHandler.Invoke(_TargetCookingStation);
			}
		}

	#endregion

	#region Member Functions
		
		public void MoveToNode(GridPosition cellToMoveTo, ANode nodeToMoveTo)
		{
			_Mover.SetDestination(cellToMoveTo);
			var cookingStation = nodeToMoveTo.GetComponent<CookingStation>();
			if (cookingStation != null)
			{
				_TargetCookingStation = cookingStation;
			}
		}

		public void PickUpIngredient(IngredientMinion ingredient)
		{
			for (int i = 0; i < _IngredientInventorySlots.Count; ++i)
			{
				if (_IngredientInventorySlots[i] == null)
				{
					PickedUpIngredientEvent.Invoke();
					ingredient.OnPickedUp(transform);
					return;
				}
			}
		}

		public void Cook(IngredientMinion ingredient)
		{
			_TargetCookingStation.Use(ingredient);
			_TargetCookingStation = null;
		}

	#endregion
}