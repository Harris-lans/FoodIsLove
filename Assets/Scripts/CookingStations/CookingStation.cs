using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class CookingStation : ANode
{

    #region Member Variables

        public static List<CookingStation> CookingStations;

        [Header("Cooking Station Properties")] 
        public string Name;
        [SerializeField] 
        private float CooldownTime;
        [SerializeField]
        private float CookingTime;
        public SO_Tag CookingStepPerformed;

        [Space, Header("Cooking Station State Details")]
        public CookingStationState State;

        [Space, Header("Cooking Station State Events")]
        public UnityEvent StationInUseEvent;
        public UnityEvent StationInCoolDownEvent;
        public UnityEvent StationIsAvailableEvent;
        public event IngredientPickedUpAction IngredientPickedUpEvent;
        [SerializeField]
        private SO_GenericEvent _IngredientStartedToCook; 
        [SerializeField]
        private SO_GenericEvent _IngredientCookedEvent;
        [SerializeField]
        private SO_GenericEvent _IngredientPickedUpEvent;
        
        private CookingStationUI _CookingStationUI;
        private SO_Tag _CookedIngredient;
        private Animator _Animator;

    #endregion

    #region Life Cycle

        private void Start()
        {
            State = CookingStationState.AVAILABLE;
            _CookingStationUI = GetComponentInChildren<CookingStationUI>();
            _Animator = GetComponent<Animator>();
        }

        private void OnEnable() 
        {
            if (CookingStations == null)
            {
                CookingStations = new List<CookingStation>();
            }
            CookingStations.Add(this);
        }

        private void OnDisable() 
        {
            CookingStations.Remove(this);
        }

    #endregion

    #region Member Functions

        public bool Use(IngredientMinion minion)
        {
            if (State != CookingStationState.AVAILABLE)
            {
                return false;
            }

            State = CookingStationState.UNAVAILABLE;
            _CookingStationUI.UpdateUI();
            StationInUseEvent.Invoke();
            _IngredientStartedToCook.Invoke(null);
            StartCoroutine(CookingDelay(CookingTime, minion));
            return true;
        }

        public void PickUpCookedFood(int playerViewID)
        {
            if (_CookedIngredient == null)
            {
                return;
            }
            
            OnPickedUpFood(playerViewID);

            Debug.LogFormat("Person who picked it up: {0}", playerViewID);
            _CookedIngredient = null;
            _IngredientPickedUpEvent.Invoke(null);

        }

    #endregion

    #region Coroutines

        protected IEnumerator CookingDelay(float cookingTime, IngredientMinion minion)
	    {
		    yield return new WaitForSeconds(cookingTime);

            _CookedIngredient = minion.Tag;
            _IngredientCookedEvent.Invoke(null);

		    State = CookingStationState.COOKED_FOOD_AVAILABLE;
            _CookingStationUI.UpdateUI();
	    }

        protected void OnPickedUpFood(int playerWhoPickedUp)
        {
            State = CookingStationState.COOLDOWN;
		    _CookingStationUI.UpdateUI();

            // Invoking Events
            if (IngredientPickedUpEvent != null) { IngredientPickedUpEvent.Invoke(playerWhoPickedUp, _CookedIngredient, CookingStepPerformed); }

            StationInCoolDownEvent.Invoke();
            StartCoroutine(CooldownDelay(CooldownTime));
        }

	    protected IEnumerator CooldownDelay(float cooldownTime)
	    {
		    yield return new WaitForSeconds(cooldownTime);

		    State = CookingStationState.AVAILABLE;
		    _CookingStationUI.UpdateUI();

	        StationIsAvailableEvent.Invoke();
	    }

    #endregion

    #region Properties

        public bool IsAvailable
	    {
		    get
		    {
			    return State == CookingStationState.AVAILABLE;
		    }
	    }

        public bool IsCookedAndReady
        {
            get
            {
                return State == CookingStationState.COOKED_FOOD_AVAILABLE;
            }
        }

	    public bool IsOnCooldown
	    {
		    get
		    {
			    return State == CookingStationState.COOLDOWN;
		    }
	    }

        public bool StationInUse
        {
            get
            {
                return State == CookingStationState.UNAVAILABLE;
            }
        }

    #endregion

    #region Enums

        [System.Serializable]
        public enum CookingStationState : byte
        {
            AVAILABLE = 0,
            UNAVAILABLE,
            COOKED_FOOD_AVAILABLE,
            COOLDOWN,
            DISRUPTED
        }

    #endregion

    public delegate void IngredientPickedUpAction(int playerWhoPickedUp, SO_Tag ingredientTag, SO_Tag cookingMethodTag);
}
