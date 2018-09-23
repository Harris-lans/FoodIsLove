using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinionUISlot : MonoBehaviour 
{
	#region Member Variables

		[SerializeField]
		private SO_UIMinionSlot _UISlotData;

		[SerializeField]
		private bool _CanDisplayData;

		[Space, Header("Events to listen to an invoke")]
		[SerializeField]
		private SO_GenericEvent _IngredientSelectEventHandler;
		[SerializeField]
		private SO_GenericEvent _HeroNearCookingStationEvent;
        [SerializeField]
        private SO_GenericEvent _HeroMovedAwayFromStationEvent;
        [SerializeField]
        private SO_GenericEvent _IngredientModifiedEvent;

		private Button _MinionButton;
		private Image _IngredientImage;
		private Color _DisabledColor;
		private Color _NormalColor;
        private bool _CanCook;
        private Sprite _DefaultSprite;

	#endregion 

	#region Life Cycle

        private void Awake()
        {
            _UISlotData.Initialize();
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
			if (_UISlotData.Ingredient == null)
			{
				DeHighlightButton();
				return;
			}

			CookingStation cookingStation = (CookingStation)data;

			// Check if the ingredient is compatible
			if (_UISlotData.Ingredient.CheckIfCompatible(cookingStation.CookingStepPerformed))
			{
				HighlightButton();
			}

			else
			{
				DeHighlightButton();
			}
		}

        private void OnHeroMovedAwayFromCookingStation(object data)
        {
            DeHighlightButton();
        }

		private void HighlightButton()
		{
			_IngredientImage.color = _NormalColor;
		    _CanCook = true;
		}

		private void DeHighlightButton()
		{
		    _IngredientImage.color = _DisabledColor;
		    _CanCook = false;
		}

		private void UpdateUIData()
		{
		    if (_UISlotData.Ingredient == null)
		    {
		        _IngredientImage.sprite = _DefaultSprite;
		        return;
		    }

			_IngredientImage.sprite = _UISlotData.Ingredient.Thumbnail;
		}

		private void OnIgredientModified(object data)
		{
			UpdateUIData();
		}

	#endregion
}
