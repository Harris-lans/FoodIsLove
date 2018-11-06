using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientContainer : MonoBehaviour 
{
    #region Member Variables

        [Header("References")]
        [SerializeField]
        private CookingStepsIcon _CookingStepsIconPrefab;
        [SerializeField]
        private Image _IngredientImage;
        [SerializeField]
        private RectTransform _StepsContainer;

        [Header("Events to listen to")]
        [SerializeField]
        private SO_GenericEvent _IngredientAddedToCookingPotEvent;

        public SO_Tag Ingredient {get; private set;}
        public List<CookingStepsIcon> CookingStepsIcon {get; private set;}
        private SO_Tag[] _CookingStepsToTrack;
        private SO_IngredientData _IngredientData;

    #endregion 

    #region Life Cycle

        private void Awake()
        {
            _IngredientData = Resources.Load<SO_IngredientData>("IngredientsData");
        }

        private void Start() 
        {
            _IngredientAddedToCookingPotEvent.AddListener(OnIngredientAddedToCookingPot);
        }

    #endregion

    #region Member Functions

        public void Initialize(SO_Tag ingredient, SO_Tag[] cookingSteps) 
        {
            CookingStepsIcon = new List<CookingStepsIcon>();
            _CookingStepsToTrack = cookingSteps;
            Ingredient = ingredient;

            foreach(var cookingStep in _CookingStepsToTrack)
            {
                // Adding step icons to the container
                var cookingStepIcon = Instantiate(_CookingStepsIconPrefab, _StepsContainer); 
                cookingStepIcon.Initialize(cookingStep);
                CookingStepsIcon.Add(cookingStepIcon);
            }

            // Displaying the ingredient icon
            _IngredientImage.sprite = _IngredientData.GetIngredientIcon(Ingredient);
        }

        private void OnIngredientAddedToCookingPot(object cookingStepData)
        {
            // Check if the ingredient matches and if it matches update the cooking step
            CookedIngredient cookedIngredient = (CookedIngredient)cookingStepData;

            // Checking if this was the ingredient cooked
            if ( cookedIngredient != null && cookedIngredient.Ingredient == Ingredient)
            {
                // Also checking if all the ingredient steps are completed
                bool ingredientCompletelyCooked = false;

                foreach(var cookingStepIcon in CookingStepsIcon)
                {
                    cookingStepIcon.ValidateAndUpdate(cookedIngredient.CookingMethod);
                    ingredientCompletelyCooked &= cookingStepIcon.IsCompleted;
                }

                if (ingredientCompletelyCooked)
                {
                    MarkAsCompleted();
                }
            }
        }

        private void MarkAsCompleted()
        {
            // TODO: Grey out the ingredient image to show that it has been cooked
            _IngredientImage.color = Color.gray;

            // Stopping to listen to the Cooking Pot event
            _IngredientAddedToCookingPotEvent.RemoveListener(OnIngredientAddedToCookingPot);
        }
    #endregion
}
