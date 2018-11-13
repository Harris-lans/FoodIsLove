using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioManager
{

    public static void PostEvent(string eventName, GameObject go)
    {
        AkSoundEngine.PostEvent(eventName, go);
    }

    public static void SetSwitch(string switchGroup, string switchState, GameObject go)
    {
        AkSoundEngine.SetSwitch(switchGroup, switchState, go);
    }

    public static void SetRTPCValue(string rtpcName, float rtpcValue, GameObject go)
    {
        AkSoundEngine.SetRTPCValue(rtpcName, rtpcValue, go);
    }

    public static void SetState(string stateGroup, string state)
    {
        AkSoundEngine.SetState(stateGroup, state);
    }
}
