using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WWiseAudioPlayer : MonoBehaviour 
{

    [SerializeField]
    private SO_WWiseSwitchTrigger _DefaultSwitchTrigger;

    private void Awake()
    {
        SwitchWWiseEvent(_DefaultSwitchTrigger);
    }

    public void TriggerWWiseEvent(string eventName)
    {
        AkSoundEngine.PostEvent(eventName, gameObject);
    }

    public void SwitchWWiseEvent(SO_WWiseSwitchTrigger switchTrigger)
    {
        AkSoundEngine.SetSwitch(switchTrigger.SwitchGroup, switchTrigger.SwitchGroup, gameObject);
    }

}
