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

		[Space, Header("Events")]
		[SerializeField]
		private SO_GenericEvent _IngredientSelectEventHandler;
		[SerializeField]
		private SO_GenericEvent _HeroNearCookingStationEvent;

		private IngredientMinion _IngredientMinion;
		private Text _Name;
		private Button _MinionButton;
		private Color _DisabledColor;
		private Color _NormalColor;

	#endregion 

	#region Life Cycle

		private void Start()
		{
			_Name = GetComponentInChildren<Text>();
			_MinionButton = GetComponent<Button>();

			_NormalColor = _MinionButton.colors.normalColor;
			_DisabledColor = _MinionButton.colors.disabledColor;

			_HeroNearCookingStationEvent.AddListener(OnHeroNearCookingStation);

			// Initial data to display
			_Name.text = "";

			DeHighlightButton();
		}

	#endregion

	#region Member Functions


		public void OnClick()
		{
			_IngredientSelectEventHandler.Invoke(_UISlotData.Ingredient);
		}

		private void OnHeroNearCookingStation(object data)
		{
			if (_IngredientMinion == null)
			{
				return;
			}

			CookingStation cookingStation = (CookingStation)data;

			// Check if the ingredient is compatible
			if (_IngredientMinion.CheckIfCompatible(cookingStation.CookingStepPerformed))
			{
				HighlightButton();
			}

			else 
			{
				DeHighlightButton();
			}
		}

		private void HighlightButton()
		{
			_MinionButton.interactable = true;
			_MinionButton.GetComponent<Image>().color = _NormalColor;
		}

		private void DeHighlightButton()
		{
			_MinionButton.interactable = false;
			_MinionButton.GetComponent<Image>().color = _DisabledColor;
		}

	#endregion
}
