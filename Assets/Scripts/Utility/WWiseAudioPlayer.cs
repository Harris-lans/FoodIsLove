using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WWiseAudioPlayer : MonoBehaviour 
{
    public void TriggerWWiseEvent(string eventName)
    {
        AkSoundEngine.PostEvent(eventName, gameObject);
    }
}
