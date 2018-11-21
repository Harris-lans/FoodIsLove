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
		if (_EventActionPairsDictionary.ContainsKey(Events.ON_ENABLE))
		{
			_EventActionPairsDictionary[Events.ON_ENABLE].Invoke();
		}
	}

	private void Awake()
	{
		PopulateDictionary();

		if (_EventActionPairsDictionary.ContainsKey(Events.AWAKE))
		{
			_EventActionPairsDictionary[Events.AWAKE].Invoke();
		}
	}

	private void Start()
	{
		if (_EventActionPairsDictionary.ContainsKey(Events.START))
		{
			_EventActionPairsDictionary[Events.START].Invoke();
		}
	}

	private void OnDestroy()
	{
		if (_EventActionPairsDictionary.ContainsKey(Events.ON_DESTROY))
		{
			_EventActionPairsDictionary[Events.ON_DESTROY].Invoke();
		}
	}

	private void OnDisable()
	{
		if (_EventActionPairsDictionary.ContainsKey(Events.ON_DISABLE))
		{
			_EventActionPairsDictionary[Events.ON_DISABLE].Invoke();
		}
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