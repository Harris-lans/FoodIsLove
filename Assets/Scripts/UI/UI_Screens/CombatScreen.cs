﻿using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class CombatScreen : UIScreen 
{
    #region Member Variables
	
		[Space, Header("Combat Data")]
		[SerializeField]
		private SO_CombatData _CombatData;

		[Space, Header("UI Elements")]
		[SerializeField]
		private Text _ResultsText;
		[SerializeField]
		private RectTransform _Clock;
		[SerializeField]
		private Image _ClockFill;

		[Header("Clash screen details")]
		[SerializeField]
		private Animator _LeftClashAnimator;

		[SerializeField]
		private Animator _RightClashAnimator;

		private Coroutine _ClockTimerCoroutine;
	
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
			_CombatData.CombatTimerStartedEvent.AddListener(OnStartCombatTimerEvent);
		}

		private void OnDisable() 
		{
			_ResultsText.enabled = false;
			_CombatData.ShowCombatResultsEvent.RemoveListener(OnShowCombatResults);
			_CombatData.CombatSequenceStartedEvent.RemoveListener(OnCombatStartedOrRestarted);
			_CombatData.CombatSequenceRestartedEvent.RemoveListener(OnCombatStartedOrRestarted);
			_CombatData.CombatTimerStartedEvent.RemoveListener(OnStartCombatTimerEvent);	
		}

		private void OnMatchStarted(object data)
		{

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
			else if (winner == -1)
			{
				_ResultsText.text = "You ran out of time!";
			}

			LocalPlayerController winnerPlayer = PhotonView.Find(winner).GetComponent<LocalPlayerController>();

			if (winnerPlayer == null)
			{
				_ResultsText.text = "You Lose";
				return;
			}
			_ResultsText.text = "You Win";
		}

		private void OnStartCombatTimerEvent(object data)
		{
			StopAllCoroutines();
			ShowClock();
			float timeLimit = (float)data;
			_ClockTimerCoroutine = StartCoroutine(StartClock(timeLimit));
		}

		private void OnCombatOptionSelected(object data)
		{
			// Checking if the timer is still running
			if (_ClockTimerCoroutine == null)
			{
				return;
			}
			HideClock();
			StopAllCoroutines();
		}	

		private IEnumerator StartClock(float timeLimit)
		{
			float timeEstablished = 0;
			while(timeEstablished <= timeLimit)
			{
				yield return null;
				timeEstablished += Time.deltaTime;
				_ClockFill.fillAmount = timeEstablished / timeLimit;
			}
			_CombatData.CombatTimerEndedEvent.Invoke(null);
			HideClock();
		}

		private void HideClock()
		{
			_Clock.gameObject.SetActive(false);
			_ClockFill.fillAmount = 0;
		}

		private void ShowClock()
		{
			_Clock.gameObject.SetActive(true);
			_ClockFill.fillAmount = 0;
		}

	#endregion
}
