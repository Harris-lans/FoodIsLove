using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WWiseAudioPlayer : MonoBehaviour 
{

    [SerializeField]
    private string _SwitchGroup;
    [SerializeField]
    private string _DefaultSwitchState;

    private void Start()
    {
        SwitchWWiseEvent(_DefaultSwitchState);
    }

    public void TriggerWWiseEvent(string eventName)
    {
        AkSoundEngine.PostEvent(eventName, gameObject);
    }

    public void SwitchWWiseEvent(string switchState)
    {
        AkSoundEngine.SetSwitch(_SwitchGroup, switchState, gameObject);
    }

}
