using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircularProgressbar : MonoBehaviour 
{
	#region Member Variables

		[Header("UI Elements")]
		[SerializeField]
		private Image _Fill;

		[Space, Header("Progressbar Properties")]
		[SerializeField]
		private float _TimeInterval = 0.15f;

	#endregion

	#region Member Functions

		private IEnumerator StartTimer(float time)
		{
			float timeCompleted = 0;
			while (timeCompleted <= time)
			{
				yield return new WaitForSeconds(_TimeInterval);
				timeCompleted += _TimeInterval;
				float ratio = timeCompleted / time;
				_Fill.fillAmount = ratio;
			}
		}

		public void StartCountDown(float time)
		{
			StopAllCoroutines();
			StartCoroutine(StartTimer(time));
		}

		public void StopTimer()
		{
			StopAllCoroutines();
		}

	#endregion
}
