﻿using System.Collections;
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
		[SerializeField]
		private Image _StepImage;
		[SerializeField]
		private float _TimeDelayBeforeDisplayingTick = 2;

		[HideInInspector]
		public bool IsCompleted;
		public SO_Tag CookingStep;
		private CookingStepData _CookingStepData;
		private SO_CookingStepsData _CookingStepsData;

	#endregion

	#region Life Cycle

		private void Awake() 
		{
			IsCompleted = false;
		}

	#endregion

	#region Member Functions

		public void Initialize(SO_Tag cookingMethod)
		{
			CookingStep = cookingMethod;
			_CookingStepsData = Resources.Load<SO_CookingStepsData>("CookingStepsData");
			_CookingStepData.CookingStep = cookingMethod;
			_CookingStepData.Icon = _CookingStepsData.GetCookingStepSprite(cookingMethod);

			// Displaying the image
			_StepImage.sprite = _CookingStepData.Icon;
		}

		private IEnumerator MarkAsCompleted()
		{
			yield return new WaitForSeconds(_TimeDelayBeforeDisplayingTick);
			_TickImage.sprite = _TickSprite;
		}

		public void ValidateAndUpdate(SO_Tag cookingStep)
		{
			if (!IsCompleted && cookingStep == _CookingStepData.CookingStep)
			{
				IsCompleted = true;
				StartCoroutine(MarkAsCompleted());
			}
		}

		public bool Validate(SO_Tag cookingStep)
		{
			return (!IsCompleted && cookingStep == _CookingStepData.CookingStep);
		}

	#endregion
}
