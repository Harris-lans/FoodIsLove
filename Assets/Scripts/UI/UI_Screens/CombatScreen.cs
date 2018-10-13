using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatScreen : UIScreen 
{
    #region Member Variables
	
		[Header("Clash screen details")]
		[SerializeField]
		private Animator _LeftClashAnimator;

		[SerializeField]
		private Animator _RightClashAnimator;
	
	#endregion

	#region Life Cycle

		private void OnEnable() 
		{
            // Setting the right clash animations depending up on the heroes picked by the players
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

			_LeftClashAnimator.SetBool("IsOnTheRight", false);
			_RightClashAnimator.SetBool("IsOnTheRight", true);
		}

	#endregion
}
