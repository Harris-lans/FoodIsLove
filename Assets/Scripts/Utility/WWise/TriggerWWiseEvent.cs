using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWWiseEvent : MonoBehaviour 
{
	[SerializeField]
	private WWiseEventData _WWiseEventData;
	[SerializeField]
	private SO_GenericEvent _EventToTriggerOn;
	[SerializeField]
	private float _DelayBeforeTriggeringEvent = 0;

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
		StartCoroutine(TriggerEventOnDelay());
	}

	private IEnumerator TriggerEventOnDelay()
	{
		yield return new WaitForSeconds(_DelayBeforeTriggeringEvent);
		AkSoundEngine.PostEvent(_WWiseEventData.EventName, _WWiseEventData.TargetGameObject);
	}
}

[System.Serializable]
public struct WWiseEventData
{
    public string EventName;
    public GameObject TargetGameObject;
    private SO_GenericEvent _EventToTriggerOn;
}

