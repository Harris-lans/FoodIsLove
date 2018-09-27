using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAudioMovement : MonoBehaviour 
{
	[Header("Timing Details")]
	[SerializeField]
	private float _IntervalBetweenInvokingMovementAudio = 3;
	private GridMover _Mover;

	[Space, Header("WWise Audio Details")]
	[SerializeField]
	private string _WWiseEventName;

	private	void Start()
	{
		_Mover = GetComponent<GridMover>();
		StartCoroutine(PlayMovementSound());
	}

	private IEnumerator PlayMovementSound()
	{
		while(true)
		{
			if (!_Mover.ReachedDestination())
			{
				AudioManager.PostEvent(_WWiseEventName, gameObject);
			}
			yield return new WaitForSeconds(_IntervalBetweenInvokingMovementAudio);
		}
	}
}
