using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvokeGenericEvent : MonoBehaviour 
{
	[SerializeField]
	private SO_GenericEvent Event;

	public void Invoke(object data)
	{
		Event.Invoke(data);
	}
}
