using System;
using System.Collections;
using System.Collections.Generic;
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

        [Space, Header("Ingredient State Details")]
        public bool IsCooked;

        [Space, Header("Ingredient Events")]
        public UnityEvent MinionCookedEvent;
        [SerializeField]
        private SO_GenericEvent _UncookedIngredientDestroyedEventHandler;

        [Space, Header("Cooking Instructions")]
        public List<SO_Tag> CookingStationStepsToPerform;
        public List<SO_Tag> CookingStationStepsPerformed;

    #endregion

    #region Life Cycle

        private void Start()
        {
            IsCooked = false;
        }

       private void OnDestroy()
       {
           if (!IsCooked)
           {
               _UncookedIngredientDestroyedEventHandler.Invoke(Tag);
           }
       }

    #endregion

    #region Member Functions

        public void Cook(CookingStation cookingStation, SO_Tag cookingStepPerformed)
        {
            cookingStation.Use(this); 
            
            // Recording the task performed
            CookingStationStepsPerformed.Add(cookingStepPerformed);
            CookingStationStepsToPerform.Remove(cookingStepPerformed);

            GetCooked();
        }

        public bool CheckIfCompatible(SO_Tag cookingStep)
        {
            // Checking if the cooking steps can be performed on the Ingredient
            return CookingStationStepsToPerform.Contains(cookingStep);
        }

        public void GetCooked()
        {
            // Changing Stats
            --_CurrentHP;
            IsCooked = true;

            if (_CurrentHP <= 0)
            {
                IngredientDie();
            }
        }

        public void OnPickedUp(Transform hero)
        {
            transform.parent = hero;
            transform.position = Vector3.zero;
            gameObject.SetActive(false);
        }

    #endregion
}
