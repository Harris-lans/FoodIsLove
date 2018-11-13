using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRateLimiter : MonoBehaviour 
{
	[SerializeField]
	private int _FrameRateLimit = 60;

	// Use this for initialization
	private void Awake () 
	{
		Application.targetFrameRate = _FrameRateLimit;
	}
}
