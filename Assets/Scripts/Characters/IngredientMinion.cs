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
        private SO_GenericEvent _IngredientWastedEvent;

        [Space, Header("Cooking Instructions")]
        public List<SO_Tag> CookingStationStepsToPerform;
        public List<SO_Tag> CookingStationStepsPerformed;

        [Space, Header("Ingredient UI Data")]
        public Sprite Thumbnail;

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
                _IngredientWastedEvent.Invoke(Tag);
            }
        }

    #endregion

    #region Member Functions

        public void Cook(int playerWhoIsCooking, CookingStation cookingStation, SO_Tag cookingStepPerformed)
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
            IsCooked = true;
            IngredientDie();
        }

    #endregion
}
