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
        private float _CooldownTime;
        [SerializeField]
        private float _CookingTime;
        public SO_Tag CookingStepPerformed;

        [Space, Header("Cooking Station State Details")]
        public CookingStationState State;

        [Space, Header("Local Events")]
        [SerializeField]
        private UnityEvent _StationInUseEvent;
        [SerializeField]
        private UnityEvent _IngredientFinishedCookingEvent;
        public UnityEvent StationInCoolDownEvent;
        [SerializeField]
        private UnityEvent _StationIsAvailableEvent;
        [SerializeField]
        private UnityEvent _OnLocalPlayerCollectedCorrectCookedFood;
        [SerializeField]
        private UnityEvent _CompatibleIngredientCollectedEvent;
        public event IngredientPickedUpAction IngredientPickedUpEvent;

        [Space, Header("Particles")]
        [SerializeField]
        private ParticleGuide _GuidedParticles;

        [Space, Header("Global Events")]
        [SerializeField]
        private SO_GenericEvent _IngredientStartedToCook; 
        [SerializeField]
        private SO_GenericEvent _IngredientCookedEvent;
        [SerializeField]
        private SO_GenericEvent _IngredientModifiedEvent;
        [SerializeField]
        private SO_GenericEvent _IngredientCollectedEvent;

        [Space, Header("Local Inventory Slots and other Data")]
        [SerializeField]
        private SO_UIMinionSlot[] _InventorySlots;
        [SerializeField]
        private SO_IngredientContainerReference[] _IngredientContainerReferences;
        
        [HideInInspector]
        private CookingStationUI _CookingStationUI;
        private SO_Tag _CookedIngredient;
        private Animator _Animator;
        private CircularProgressbar _Clock;

    #endregion

    #region Life Cycle

        private void Awake()
        {
            State = (CheckIfTheIngredientsInInventoryAreCompatible()) ? CookingStationState.AVAILABLE : CookingStationState.NOT_VISIBLE_TO_LOCAL_PLAYER;
            _CookingStationUI = GetComponentInChildren<CookingStationUI>();
            _Animator = GetComponent<Animator>();
            _Clock = GetComponentInChildren<CircularProgressbar>(true);
        }

        private void OnEnable() 
        {
            if (CookingStations == null)
            {
                CookingStations = new List<CookingStation>();
            }
            CookingStations.Add(this);
            _IngredientCollectedEvent.AddListener(OnCollectedIngredient);
            _IngredientModifiedEvent.AddListener(OnIngredientModified);
        }

        private void OnDisable() 
        {
            CookingStations.Remove(this);
            _IngredientCollectedEvent.RemoveListener(OnCollectedIngredient);
            _IngredientModifiedEvent.RemoveListener(OnIngredientModified);
        }

    #endregion

    #region Member Functions

        public bool Use(IngredientMinion minion)
        {
            if (State != CookingStationState.AVAILABLE && State != CookingStationState.NOT_VISIBLE_TO_LOCAL_PLAYER)
            {
                return false;
            }

            State = CookingStationState.COOKING;
            _CookingStationUI.UpdateUI();
            _StationInUseEvent.Invoke();
            _IngredientStartedToCook.Invoke(null);
            ShowClock();
            StartCoroutine(CookingDelay(_CookingTime, minion));
            return true;
        }

        public void PickUpCookedFood(int playerViewID, bool isLocalPlayer)
        {
            if (_CookedIngredient == null)
            {
                return;
            }
            
            OnPickedUpCookedFood(playerViewID, isLocalPlayer);
            _CookedIngredient = null;
        }

        private void ShowClock()
        {
            _Clock.gameObject.SetActive(true);
            _Clock.StartCountDown(_CookingTime);
        }

        private void HideClock()
        {
            _Clock.StopTimer();
            _Clock.gameObject.SetActive(false);
        }

        private void OnCollectedIngredient(object data)
        {
            SO_UIMinionSlot slot = (SO_UIMinionSlot)data;

            if (State == CookingStationState.AVAILABLE || State == CookingStationState.NOT_VISIBLE_TO_LOCAL_PLAYER)
            {
                if (CheckIfTheIngredientsInInventoryAreCompatible())
                {
                    State = CookingStationState.AVAILABLE;
                    _CompatibleIngredientCollectedEvent.Invoke();
                }
                else
                {
                    State = CookingStationState.NOT_VISIBLE_TO_LOCAL_PLAYER;
                }
            }
            _CookingStationUI.UpdateUI();
        }

        private void OnIngredientModified(object data)
        {
            // Disabling if there are no compatible ingredients
            if (State != CookingStationState.AVAILABLE && State != CookingStationState.NOT_VISIBLE_TO_LOCAL_PLAYER)
            {
                return;
            }
            State = (CheckIfTheIngredientsInInventoryAreCompatible()) ? CookingStationState.AVAILABLE : CookingStationState.NOT_VISIBLE_TO_LOCAL_PLAYER;
            _CookingStationUI.UpdateUI();
        } 

        private bool CheckIfTheIngredientsInInventoryAreCompatible()
        {
            foreach (var inventorySlot in _InventorySlots)
            {
                if (inventorySlot.Ingredient != null && inventorySlot.Ingredient.CheckIfCompatible(CookingStepPerformed))
                {
                    return true;
                }
            }
            return false;
        }

        protected void OnPickedUpCookedFood(int playerWhoPickedUp, bool isLocalPlayer)
        {
            State = CookingStationState.COOLDOWN;
		    _CookingStationUI.UpdateUI();

            // Invoking Events
            if (IngredientPickedUpEvent != null) 
            { 
                IngredientPickedUpEvent.Invoke(playerWhoPickedUp, _CookedIngredient, CookingStepPerformed); 
            }

            if (isLocalPlayer)
            {
                OnLocalPlayerCollectedCookedFood(_CookedIngredient);
            }

            StationInCoolDownEvent.Invoke();
            StartCoroutine(CooldownDelay(CooldownTime));
        }

        private void OnLocalPlayerCollectedCookedFood(SO_Tag collectedIngredient)
        {
            foreach(var ingredientContainer in _IngredientContainerReferences)
            {
                if (ingredientContainer.Reference.Ingredient == collectedIngredient)
                {
                    foreach (var cookingStep in ingredientContainer.Reference.CookingStepsIcon)
                    {
                        if (cookingStep.CookingStep == CookingStepPerformed)
                        {
                            var particles = Instantiate(_GuidedParticles, transform.position, _GuidedParticles.transform.rotation);
                            particles.InitiateParticleFlow(cookingStep.transform);
                            return;
                        }
                    }
                }
            }
        }

    #endregion

    #region Coroutines

        protected IEnumerator CookingDelay(float cookingTime, IngredientMinion minion)
	    {
		    yield return new WaitForSeconds(cookingTime);
            
            HideClock();
            minion.GetCooked();
            _CookedIngredient = minion.Tag;
            _IngredientCookedEvent.Invoke(null);
            _IngredientFinishedCookingEvent.Invoke();
		    State = CookingStationState.COOKED_FOOD_AVAILABLE;
            _CookingStationUI.UpdateUI();
	    }

	    protected IEnumerator CooldownDelay(float cooldownTime)
	    {
		    yield return new WaitForSeconds(cooldownTime);
		    State = (CheckIfTheIngredientsInInventoryAreCompatible()) ? CookingStationState.AVAILABLE : CookingStationState.NOT_VISIBLE_TO_LOCAL_PLAYER;
		    _CookingStationUI.UpdateUI();
	        _StationIsAvailableEvent.Invoke();
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

        public float CooldownTime
        {
            get
            {
                return _CooldownTime;
            }
        }

    #endregion

    #region Enums

        [System.Serializable]
        public enum CookingStationState : byte
        {
            UNAVAILABLE = 0,
            AVAILABLE,
            COOKING,
            COOKED_FOOD_AVAILABLE,
            COOLDOWN,
            NOT_VISIBLE_TO_LOCAL_PLAYER
        }

    #endregion

    public delegate void IngredientPickedUpAction(int playerWhoPickedUp, SO_Tag ingredientTag, SO_Tag cookingMethodTag);
}
