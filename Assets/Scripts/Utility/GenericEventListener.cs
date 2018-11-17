using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericEventListener : MonoBehaviour 
{
	[Header("Event to listen to")]
	[SerializeField]
	private SO_GenericEvent _EventToListenTo;

	[Space, Header("Actions to do")]
	[SerializeField]
	private UnityEvent _ActionsToPerformOnEvent;

	private void OnEnable()
	{
		_EventToListenTo.AddListener(OnEvent);
	}

	private void OnDisable() 
	{
		_EventToListenTo.RemoveListener(OnEvent);
	}

	private void OnEvent(object data)
	{
		_ActionsToPerformOnEvent.Invoke();
	}
}
