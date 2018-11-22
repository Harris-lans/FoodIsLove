using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class IngredientMinion : Ingredient
{
    #region Member Variables

        [Header("Minon Specific Stats")]
        [SerializeField]
        private float _HeroBonus;
        [SerializeField]
        private float _StationEffective;

        [Space, Header("Ingredient Minion Details")] 
        public SO_Tag Tag;
        [SerializeField]
        private float _TimeBeforeIngredientsDissapear = 5;

        [Space, Header("Ingredient State Details")]
        public bool IsCooked;

        [Space, Header("Ingredient Events")]
        public UnityEvent MinionStartedCookingEvent;
        public UnityEvent MinionCookedEvent;
        public UnityEvent PickedUpEvent;
        public UnityEvent IngredientExpiredEvent;

        [SerializeField]
        private SO_GenericEvent _IngredientWastedEvent;

        [Space, Header("Cooking Instructions")]
        public List<SO_Tag> CookingStationStepsToPerform;
        public List<SO_Tag> CookingStationStepsPerformed;

        [Space, Header("Ingredient UI Data")]
        public Sprite Thumbnail;

        private bool _IsPickedUp;

    #endregion

    #region Life Cycle

        private void Start()
        {
            IsCooked = false;
            _IsPickedUp = false;
            StartCoroutine(CountDownToDissapear());
        }

        private void OnDestroy()
        {
            if (!IsCooked && PhotonNetwork.IsMasterClient)
            {
                Debug.Log("Ingredient wasted event");
                _IngredientWastedEvent.Invoke(Tag);
            }
        }

    #endregion

    #region Member Functions

        public void Cook(CookingStation cookingStation, SO_Tag cookingStepPerformed)
        {
            cookingStation.Use(this); 
            MinionStartedCookingEvent.Invoke();

            // Recording the task performed
            CookingStationStepsPerformed.Add(cookingStepPerformed);
            CookingStationStepsToPerform.Remove(cookingStepPerformed);

            // Listening to cooked event
            cookingStation.StationInCoolDownEvent.AddListener(OnIngredientCooked);

            MinionCookedEvent.Invoke();
            
            GetCooked(CheckIfCompatible(cookingStepPerformed));
        }

        public bool CheckIfCompatible(SO_Tag cookingStep)
        {
            // Checking if the cooking steps can be performed on the Ingredient
            return CookingStationStepsToPerform.Contains(cookingStep);
        }

        private void GetCooked(bool isCompatible)
        {
            IsCooked = isCompatible;
            IngredientDie();
        }

        public void OnPickedUp()
        {
            _IsPickedUp = true;
            PickedUpEvent.Invoke();
            PickedUpEvent.RemoveAllListeners();
        }

        public void OnIngredientCooked()
        {
            MinionCookedEvent.Invoke();
        }

        private IEnumerator CountDownToDissapear()
        {
            yield return new WaitForSeconds(_TimeBeforeIngredientsDissapear);
            if (!_IsPickedUp)
            {
                Destroy(gameObject);
                IngredientExpiredEvent.Invoke();
            }
        }

    #endregion
}
