using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventInvokers : MonoBehaviour 
{
	[SerializeField]
	private EventActionPair[] _EventActionPairs;
	private Dictionary<Events, UnityEvent> _EventActionPairsDictionary;

	private void OnEnable()
	{
		_EventActionPairsDictionary[Events.ON_ENABLE].Invoke();
	}

	private void Awake()
	{
		PopulateDictionary();
		_EventActionPairsDictionary[Events.AWAKE].Invoke();
	}

	private void Start()
	{
		_EventActionPairsDictionary[Events.START].Invoke();
	}

	private void OnDestroy()
	{
		_EventActionPairsDictionary[Events.ON_DESTROY].Invoke();
	}

	private void OnDisable()
	{
		_EventActionPairsDictionary[Events.ON_DISABLE].Invoke();
	}

	private void PopulateDictionary()
	{
		_EventActionPairsDictionary = new Dictionary<Events, UnityEvent>();

		foreach (var eventActionPair in _EventActionPairs)
		{
			_EventActionPairsDictionary[eventActionPair.EventToInvokeOn] = eventActionPair.ActionsToDo;
		}
	}
}

[System.Serializable]
public enum Events : byte
{
	AWAKE = 0,
	ON_ENABLE,
	ON_DESTROY,
	START,
	ON_DISABLE,
}

[System.Serializable]
public struct EventActionPair
{
	public Events EventToInvokeOn;
	public UnityEvent ActionsToDo;
}