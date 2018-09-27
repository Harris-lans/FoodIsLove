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
        public event IngredientCookedAction IngredientCookedEvent;
        [SerializeField]
        private SO_GenericEvent _IngredientStartedToCook; 
        [SerializeField]
        private SO_GenericEvent _IngredientCookedEvent;
        
        private CookingStationUI _CookingStationUI;
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

        public void Use(IngredientMinion minion, int playerViewID)
        {
            State = CookingStationState.UNAVAILABLE;
            _CookingStationUI.UpdateUI();
            StationInUseEvent.Invoke();
            _IngredientStartedToCook.Invoke(null);
            StartCoroutine(CookingDelay(CookingTime, minion, playerViewID));
        }

    #endregion

    #region Coroutines

        protected IEnumerator CookingDelay(float cookingTime, IngredientMinion minion, int playerViewID)
	    {
		    yield return new WaitForSeconds(cookingTime);

            _IngredientCookedEvent.Invoke(null);

		    State = CookingStationState.COOLDOWN;
		    _CookingStationUI.UpdateUI();

            // Invoking Events
            if (IngredientCookedEvent != null) { IngredientCookedEvent.Invoke(playerViewID, minion.Tag, CookingStepPerformed); }
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
            COOLDOWN,
            DISRUPTED
        }

    #endregion

    public delegate void IngredientCookedAction(int playerWhoCooked, SO_Tag ingredientTag, SO_Tag cookingMethodTag);
}
