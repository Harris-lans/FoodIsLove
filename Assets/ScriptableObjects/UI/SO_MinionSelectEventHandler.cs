using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MinionSelectUIEvent", menuName="FoodIsLove/UI/MinionSelectUIEvent")]
public class SO_MinionSelectEventHandler : ScriptableObject
{
    private event MinonSelectAction _MinionSelectEvent;

	private void Awake()
	{
		_MinionSelectEvent = null;
	}

	public void SubscribeToMinionSelectEvent(MinonSelectAction action)
	{
		_MinionSelectEvent += action;
	}

	public void InvokeEvent(LocalMinionController minionController)
	{
		if (_MinionSelectEvent != null)
		{
			_MinionSelectEvent.Invoke(minionController);
		}
	}

    public delegate void MinonSelectAction(LocalMinionController minionController);
}
