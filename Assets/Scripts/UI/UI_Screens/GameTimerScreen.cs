using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimerScreen : UIScreen 
{
	#region Member Variables

		[SerializeField]
		private SO_LobbyDetails _LobbyDetails;

		[Space, Header("Timer Details")]
		[SerializeField]
		private float _TimeBeforeStartingTheGame = 3.0f;

		[Space, Header("UI Elements")]
		[SerializeField]
		private Image _JudgeImage;

		[Space, Header("Screen to switch to")]
		[SerializeField]
		private SO_Tag _GameScreen;

	#endregion

	#region Life Cycle

		private void OnEnable()
		{
			DisplayInformation();
			StartCoroutine(StartTimer());
		}

	#endregion

	#region Member Functions

		private IEnumerator StartTimer()
		{
			int timeLeft = (int)_TimeBeforeStartingTheGame;
			while(timeLeft > 0)
			{
				yield return new WaitForSeconds(1);
				--timeLeft;
			}
			_UIManager.SetScreen(_GameScreen);
		}

		private void DisplayInformation()
		{
			_JudgeImage.sprite = _LobbyDetails.Judge.JudgeThumbnail;
		}

	#endregion
}
