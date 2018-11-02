using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MinionUISlot : MonoBehaviour 
{
	#region Member Variables

		[SerializeField]
		private SO_UIMinionSlot _UISlotData;

		[SerializeField]
		private bool _CanDisplayData;

		[Space, Header("Local Events")]
		[SerializeField]
		private UnityEvent _CollectedIngredientEvent;
		[SerializeField]
		private UnityEvent _UsedIngredientEvent;
		[SerializeField]
		private UnityEvent _IngredientAvailableEvent;
		[SerializeField]
		private UnityEvent _IngredientUnavailableEvent;

		[Space, Header("Global Events")]
		[SerializeField]
		private SO_GenericEvent _IngredientSelectEventHandler;
		[SerializeField]
		private SO_GenericEvent _HeroNearCookingStationEvent;
        [SerializeField]
        private SO_GenericEvent _HeroMovedAwayFromStationEvent;
        [SerializeField]
        private SO_GenericEvent _IngredientModifiedEvent;
		[SerializeField]
		private SO_GenericEvent _IngredientPickedUpEvent;

		private Button _MinionButton;
		private Image _IngredientImage;
		private Color _DisabledColor;
		private Color _NormalColor;
        private bool _CanCook;
        private Sprite _DefaultSprite;
		private SO_IngredientData _IngredientData;
		private Coroutine _UpdateUI;

	#endregion 

	#region Life Cycle

        private void Awake()
        {
            _UISlotData.Initialize();
			_IngredientData = Resources.Load<SO_IngredientData>("IngredientsData");
			_UpdateUI = null;
        }

		private void Start()
		{
			_MinionButton = GetComponent<Button>();
			_IngredientImage = GetComponent<Image>();

			_NormalColor = _MinionButton.colors.normalColor;
			_DisabledColor = _MinionButton.colors.disabledColor;
		    _CanCook = false;
		    _DefaultSprite = _IngredientImage.sprite;

			_HeroNearCookingStationEvent.AddListener(OnHeroNearCookingStation);
		    _HeroMovedAwayFromStationEvent.AddListener(OnHeroMovedAwayFromCookingStation);
            _IngredientModifiedEvent.AddListener(OnIgredientModified);
			_IngredientPickedUpEvent.AddListener(OnIngredientPickedUp);

			DeHighlightButton();
		}

	#endregion

	#region Member Functions


		public void OnClick()
		{
		    if (_CanCook)
		    {
                _IngredientSelectEventHandler.Invoke(_UISlotData.Ingredient);
		    }
		}

		private void OnHeroNearCookingStation(object data)
		{
			CookingStation cookingStation = (CookingStation)data;

			// Check if the ingredient is compatible and the station is available
			if (_UISlotData.Ingredient != null && _UISlotData.Ingredient.CheckIfCompatible(cookingStation.CookingStepPerformed))
			{
				StartCoroutine(UpdateUIData(cookingStation));
			}
		}

        private void OnHeroMovedAwayFromCookingStation(object data)
        {
			StopAllCoroutines();
            DeHighlightButton();
        }

		private void HighlightButton()
		{
			_IngredientImage.color = _NormalColor;
		    _CanCook = true;
			_IngredientAvailableEvent.Invoke();
		}

		private void DeHighlightButton()
		{
		    _IngredientImage.color = _DisabledColor;
		    _CanCook = false;
			_IngredientUnavailableEvent.Invoke();
		}

		private void UpdateUIData()
		{
		    if (_UISlotData.Ingredient == null)
		    {
		        _IngredientImage.sprite = _DefaultSprite;
		        return;
		    }

			_IngredientImage.sprite = _IngredientData.GetIngredientIcon(_UISlotData.Ingredient.Tag);
		}

		private void OnIgredientModified(object data)
		{
			UpdateUIData();
		}

		private void OnIngredientPickedUp(object data)
		{
			SO_UIMinionSlot ingredientSlot = (SO_UIMinionSlot)data;
			if (ingredientSlot != _UISlotData)
			{
				return;
			}
			_CollectedIngredientEvent.Invoke();
		}

	#endregion

	#region Co-Routines

		private IEnumerator UpdateUIData(CookingStation cookingStation)
		{
			HighlightButton();
			while(_UISlotData.Ingredient != null && (cookingStation.State == CookingStation.CookingStationState.AVAILABLE || cookingStation.State == CookingStation.CookingStationState.NOT_VISIBLE_TO_LOCAL_PLAYER))
			{
				yield return null;
			}
			DeHighlightButton();
		}

	#endregion
}
