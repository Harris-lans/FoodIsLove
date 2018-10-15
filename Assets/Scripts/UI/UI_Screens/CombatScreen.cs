using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class CombatScreen : UIScreen 
{
    #region Member Variables
	
		[SerializeField]
		private SO_CombatData _CombatData;

		[Space, Header("UI Elements")]
		[SerializeField]
		private Text _ResultsText;

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

			_CombatData.ShowCombatResultsEvent.AddListener(OnShowCombatResults);
			_CombatData.CombatSequenceStartedEvent.AddListener(OnCombatStartedOrRestarted);
			_CombatData.CombatSequenceRestartedEvent.AddListener(OnCombatStartedOrRestarted);
		}

		private void OnDisable() 
		{
			_ResultsText.enabled = false;
			_CombatData.ShowCombatResultsEvent.RemoveListener(OnShowCombatResults);
			_CombatData.CombatSequenceStartedEvent.RemoveListener(OnCombatStartedOrRestarted);
			_CombatData.CombatSequenceRestartedEvent.RemoveListener(OnCombatStartedOrRestarted);	
		}

		private void OnCombatStartedOrRestarted(object data)
		{
			_ResultsText.enabled = false;
		}

		private void OnShowCombatResults(object data)
		{
			_ResultsText.enabled = true;

			int[] combatData = (int[])data;
			int winner = combatData[2];

			if (winner == 0)
			{
				_ResultsText.text = "Tie";
				return;
			}

			LocalPlayerController winnerPlayer = PhotonView.Find(winner).GetComponent<LocalPlayerController>();

			if (winnerPlayer == null)
			{
				_ResultsText.text = "Loser";
				return;
			}
			_ResultsText.text = "Winner";
		}

	#endregion
}
