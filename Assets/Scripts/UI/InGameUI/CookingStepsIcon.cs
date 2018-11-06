using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookingStepsIcon : MonoBehaviour 
{
	#region Member Variables

		[SerializeField]
		private Image _TickImage;
		[SerializeField]
		private Sprite _TickSprite;

		[HideInInspector]
		public bool IsCompleted;
		public SO_Tag CookingStep;
		private Image _StepImage;
		private CookingStepData _CookingStepData;
		private SO_CookingStepsData _CookingStepsData;

	#endregion

	#region Life Cycle

		private void Awake() 
		{
			_StepImage = GetComponent<Image>();
			IsCompleted = false;
			_CookingStepsData = Resources.Load<SO_CookingStepsData>("CookingStepsData");
		}

	#endregion

	#region Member Functions

		public void Initialize(SO_Tag cookingMethod)
		{
			CookingStep = cookingMethod;
			_CookingStepData.CookingStep = cookingMethod;
			_CookingStepData.Icon = _CookingStepsData.GetCookingStepSprite(cookingMethod);

			// Displaying the image
			_StepImage.sprite = _CookingStepData.Icon;
		}

		private void MarkAsCompleted()
		{
			// TODO: Show a tick on the step
			IsCompleted = true;
			_TickImage.sprite = _TickSprite;
		}

		public void ValidateAndUpdate(SO_Tag cookingStep)
		{
			if (!IsCompleted && cookingStep == _CookingStepData.CookingStep)
			{
				MarkAsCompleted();
			}
		}

		public bool Validate(SO_Tag cookingStep)
		{
			return (!IsCompleted && cookingStep == _CookingStepData.CookingStep);
		}

	#endregion
}
