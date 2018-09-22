using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Generic Game Event", menuName = "FoodIsLove/GameEvent/GenericEevent", order = 0)]
public class SO_GenericEvent : ScriptableObject 
{
	private GenericAction _Event;

	public void Initialize()
	{
		_Event = null;
	}

	public void Invoke(object data)
	{
		if (_Event != null) { _Event.Invoke(data); }
	}

	public void AddListener(GenericAction action)
	{
		_Event += action;
	}

	public void RemoveListener(GenericAction action)
	{
		_Event -= action;
	}
}

public delegate void GenericAction(object data);
