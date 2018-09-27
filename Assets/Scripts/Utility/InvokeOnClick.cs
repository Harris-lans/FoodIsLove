using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvokeOnClick : MonoBehaviour 
{
    public void OnClick(SO_GenericEvent eventToInvoke)
    {
        eventToInvoke.Invoke(null);
    }
}
