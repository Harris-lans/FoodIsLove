using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericEventResetter : MonoBehaviour 
{
	[SerializeField]
	private SO_GenericEvent[] _GenericEvents;

	// Use this for initialization
	void Start () 
	{
		foreach(var genericEvent in _GenericEvents)
		{
			genericEvent.Initialize();
		}
	}
}
