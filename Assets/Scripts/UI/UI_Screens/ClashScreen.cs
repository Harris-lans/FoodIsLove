using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClashScreen : UIScreen 
{
	#region Member Variables
	
		[Header("Clash screen details")]
		[SerializeField]
		private Animator _LeftClashAnimator;

		[SerializeField]
		private Animator _RightClashAnimator;

		[Header("Transistion Details")]
		[SerializeField] 
		private float _TimeBeforeTransition;
		[SerializeField]
		private SO_Tag _ScreenToTransitionTo; 
	
	#endregion

	#region Life Cycle

		private void Start()
		{
			IngredientHero[] ingredientHeroes = FindObjectsOfType<IngredientHero>();
			
			foreach(var hero in ingredientHeroes)
			{
				if (hero.IsLocal)
				{
					_LeftClashAnimator.runtimeAnimatorController = hero.ClashAnimation;
				}
				else
				{
					_RightClashAnimator.runtimeAnimatorController = hero.ClashAnimation;
				}
			}

			Debug.Log(_LeftClashAnimator.runtimeAnimatorController);
			Debug.Log(_RightClashAnimator.runtimeAnimatorController);

			_LeftClashAnimator.SetBool("IsOnTheRight", false);
			_RightClashAnimator.SetBool("IsOnTheRight", true);
		}

		private void OnEnable() 
		{
			StartCoroutine(TransitionTo());
		}

	#endregion

	
	#region Member Functions

		private IEnumerator TransitionTo()
		{
			yield return new WaitForSeconds(_TimeBeforeTransition);
			_UIManager.SetScreen(_ScreenToTransitionTo);
		} 

	#endregion
}
