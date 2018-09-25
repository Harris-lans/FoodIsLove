using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWWiseEvent : MonoBehaviour 
{
	[SerializeField]
	private WWiseEventData _WWiseEventData;
	[SerializeField]
	private SO_GenericEvent _EventToTriggerOn;

	private void OnEnable()
	{
		_EventToTriggerOn.AddListener(OnEvent);
	}

	private void OnDisable()
	{
		_EventToTriggerOn.RemoveListener(OnEvent);
	}

	private void OnEvent(object data)
	{
		AkSoundEngine.PostEvent(_WWiseEventData.EventName, _WWiseEventData.TargetGameObject);
	}

	[System.Serializable]
	public struct WWiseEventData
	{
		public string EventName;
		public GameObject TargetGameObject;
	}
}
