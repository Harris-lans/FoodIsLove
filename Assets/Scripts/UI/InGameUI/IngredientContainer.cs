using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientContainer : MonoBehaviour 
{
    #region Member Variables

        [Header("Time Data")]
        [SerializeField]
        private float _TimeBeforeDisplayingTick = 2.5f;

        [Header("References")]
        [SerializeField]
        private CookingStepsIcon _CookingStepsIconPrefab;
        [SerializeField]
        private Image _IngredientImage;
        [SerializeField]
        private Image _TickImage;
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
            _TickImage.enabled = false;
        }

        private void OnEnable() 
        {
            _IngredientAddedToCookingPotEvent.AddListener(OnIngredientAddedToCookingPot);
        }

        private void OnDisable() 
        {
            _IngredientAddedToCookingPotEvent.RemoveListener(OnIngredientAddedToCookingPot);
        }

    #endregion

    #region Member Functions

        public void Initialize(SO_Tag ingredient, SO_Tag[] cookingSteps) 
        {
            CookingStepsIcon = new List<CookingStepsIcon>();
            _CookingStepsToTrack = cookingSteps;
            _IngredientData = Resources.Load<SO_IngredientData>("IngredientsData");
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
                    Debug.Log(cookingStepIcon.CookingStep);
                    cookingStepIcon.ValidateAndUpdate(cookedIngredient.CookingMethod);
                    ingredientCompletelyCooked = ingredientCompletelyCooked && cookingStepIcon.IsCompleted;
                }

                if (ingredientCompletelyCooked)
                {
                    StartCoroutine(MarkAsCompletedDelay());
                }
            }
        }

        private void MarkAsCompleted()
        {
            _TickImage.enabled = true;

            // Stopping to listen to the Cooking Pot event
            _IngredientAddedToCookingPotEvent.RemoveListener(OnIngredientAddedToCookingPot);
        }

        private IEnumerator CheckIfCookingStepsAreCompleted()
        {
            bool cookingStepsCompleted = false;
            while (!cookingStepsCompleted)
            {
                foreach (var cookingStep in CookingStepsIcon)
                {
                    cookingStepsCompleted = cookingStep.IsCompleted;
                }
                yield return new WaitForFixedUpdate();
            }
        }

        private IEnumerator MarkAsCompletedDelay()
        {
            yield return new WaitForSeconds(_TimeBeforeDisplayingTick);
            MarkAsCompleted();
        }


    #endregion
}
