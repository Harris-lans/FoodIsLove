using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WWiseAudioPlayer : MonoBehaviour 
{

    [SerializeField]
    private SO_WWiseSwitchTrigger[] _DefaultSwitchTriggers;

    private void Awake()
    {
        foreach (var switchTrigger in _DefaultSwitchTriggers)
        {
            SwitchWWiseEvent(switchTrigger);
        }
    }

    public void TriggerWWiseEvent(string eventName)
    {
        AkSoundEngine.PostEvent(eventName, gameObject);
    }

    public void TriggerWWiseEventWithDelay(SO_WWiseEventTriggerWithDelay eventDetails)
    {
        StartCoroutine(TriggerEventAfterDelay(eventDetails));
    }

    public void SwitchWWiseEvent(SO_WWiseSwitchTrigger switchTrigger)
    {
        AkSoundEngine.SetSwitch(switchTrigger.SwitchGroup, switchTrigger.SwitchState, gameObject);
    }

    private IEnumerator TriggerEventAfterDelay(SO_WWiseEventTriggerWithDelay eventDetails)
    {
        yield return new WaitForSecondsRealtime(eventDetails.DelayBeforeTriggeringEvent);
        AkSoundEngine.PostEvent(eventDetails.EventName, gameObject);
    }

}
