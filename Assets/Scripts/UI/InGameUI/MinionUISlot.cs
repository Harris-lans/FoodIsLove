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

		[Space, Header("More Data")]
		[SerializeField]
		private float _TimeBeforeRemovingIngredientsAfterHeroDied = 2.0f;

		[Space, Header("Local Events")]
		[SerializeField]
		private UnityEvent _CollectedIngredientEvent;
		[SerializeField]
		private UnityEvent _IngredientAvailableEvent;
		[SerializeField]
		private UnityEvent _IngredientUnavailableEvent;
		[SerializeField]
		private UnityEvent _IngredientRemovedEvent;

		[Space, Header("Global Events")]
		[SerializeField]
		private SO_GenericEvent _MatchStartedEvent;
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
		[SerializeField]
		private SO_GenericEvent _HeroDiedEvent;

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
			_MatchStartedEvent.AddListener(OnMatchStarted);
			_IngredientData = Resources.Load<SO_IngredientData>("IngredientsData");
			_UpdateUI = null;
        }

		private void Start()
		{
			_MinionButton = GetComponent<Button>();
			_IngredientImage = GetComponent<Image>();

			_NormalColor = _MinionButton.colors.normalColor;
			_DisabledColor = _MinionButton.colors.disabledColor;
		    _DefaultSprite = _IngredientImage.sprite;

			_HeroNearCookingStationEvent.AddListener(OnHeroNearCookingStation);
		    _HeroMovedAwayFromStationEvent.AddListener(OnHeroMovedAwayFromCookingStation);
            _IngredientModifiedEvent.AddListener(OnIgredientModified);
			_IngredientPickedUpEvent.AddListener(OnIngredientPickedUp);
			_HeroDiedEvent.AddListener(OnHeroDiedEvent);

			Initialize();
		}

	#endregion

	#region Member Functions

		private void OnMatchStarted(object data)
		{
			Initialize();
		}

		private void Initialize()
		{
			_UISlotData.Initialize();
			_CanCook = false;
			_IngredientImage.sprite = _DefaultSprite;
			DeHighlightButton();
		}

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
				if (_IngredientImage.sprite != _DefaultSprite)
				{
					_IngredientRemovedEvent.Invoke();
				}
		        _IngredientImage.sprite = _DefaultSprite;
				DeHighlightButton();
		        return;
		    }

			_IngredientImage.sprite = _IngredientData.GetIngredientIcon(_UISlotData.Ingredient.Tag);
			DeHighlightButton();
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

		private void OnHeroDiedEvent(object data)
		{
			StartCoroutine(RemoveIngredientOnHeroDied());
		}

		private IEnumerator RemoveIngredientOnHeroDied()
		{
			yield return new WaitForSeconds(_TimeBeforeRemovingIngredientsAfterHeroDied);
			UpdateUIData();
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
