using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTimer : MonoBehaviour 
{
	[SerializeField]
	private float _TimeToWaitBeforeDestroy;

	private void Start()
	{
		Destroy(gameObject, _TimeToWaitBeforeDestroy);
	}
}
