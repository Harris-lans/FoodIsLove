using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAudioMovement : MonoBehaviour 
{
	[Header("Timing Details")]
	[SerializeField]
	private float _IntervalBetweenInvokingMovementAudio = 3;
	private GridMover _Mover;

	private	void Start()
	{
		_Mover = GetComponent<GridMover>();
	}

	private IEnumerator PlayMovementSound()
	{
		while(true)
		{
			if (!_Mover.ReachedDestination())
			{
				AudioManager.PostEvent("SFX_CH_Gen_Hop", gameObject);
			}
			yield return new WaitForSeconds(_IntervalBetweenInvokingMovementAudio);
		}
	}
}
