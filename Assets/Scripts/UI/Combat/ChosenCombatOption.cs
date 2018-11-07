using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChosenCombatOption : MonoBehaviour 
{
	[Header("Combat Data")]
	[SerializeField]
	private SO_CombatData _CombatData;

	[Space, Header("Combat Option Data")]
	[SerializeField]
	private CombatOptionData[] _CombatOptionsData;
	[SerializeField]
	private bool _ShowLocalPlayerOption;

	[Space, Header("Local Events")]
	[SerializeField]
	private UnityEvent _OnShowCombatOptionEvent;

	private Image _Image;
	private Sprite _DefaultSprite;

	#region Life Cycle

		private void Start()
		{
			// Initializing variables
			_Image = GetComponent<Image>();
			_DefaultSprite = _Image.sprite;

			// Subscribing to combat events
			_CombatData.CombatSequenceStartedEvent.AddListener(OnCombatOptionStartedOrRestarted);
			_CombatData.CombatSequenceRestartedEvent.AddListener(OnCombatOptionStartedOrRestarted);
			
			if (!_ShowLocalPlayerOption)
			{
				_CombatData.ShowCombatResultsEvent.AddListener(OnShowCombatResults);
				return;
			}
			_CombatData.CombatOptionChosenEvent.AddListener(OnShowLocalCombatOption);
		}

	#endregion

	#region Member Functions

		private void OnCombatOptionStartedOrRestarted(object data)
		{
			// Showing the loading image
			_Image.sprite = _DefaultSprite;
		}

		private void OnShowCombatResults(object data)
		{
			int[] combatData = (int[])data;
			int playerViewID = combatData[0];
			CombatOptionButton.CombatOptions chosenOption = (CombatOptionButton.CombatOptions)combatData[1];

			// Show the option chosen by the net player
			NetPlayerController player = PhotonView.Find(playerViewID).GetComponent<NetPlayerController>();

			if (player == null)
			{
				return;
			}

			// Update the UI with the chosen option
			ShowChosenOption(chosenOption);
		}

		private void OnShowLocalCombatOption(object data)
		{
			Debug.Log("Showing local combat option");
			int[] combatData = (int[])data;
			int playerViewID = combatData[0];
			CombatOptionButton.CombatOptions chosenOption = (CombatOptionButton.CombatOptions)combatData[1];

			// Show the option chosen by the local player
			LocalPlayerController player = PhotonView.Find(playerViewID).GetComponent<LocalPlayerController>();

			if (player == null)
			{
				return;
			}

			// Update the UI with the chosen option
			ShowChosenOption(chosenOption);
		}

		private void ShowChosenOption(CombatOptionButton.CombatOptions chosenOption)
		{
			// Setting the appropriate image
			foreach(var combatOptionData in _CombatOptionsData)
			{
				if (combatOptionData.CombatOption == chosenOption)
				{
					_Image.sprite = combatOptionData.Image;
					_OnShowCombatOptionEvent.Invoke();
					return;
				}
			}
		}

	#endregion
}

[System.Serializable]
public struct CombatOptionData
{
	public Sprite Image;
	public CombatOptionButton.CombatOptions CombatOption;
}