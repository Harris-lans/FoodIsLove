using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnEvent : MonoBehaviour 
{
    #region Global Variables

        [SerializeField]
        private MonoBehaviour[] _ComponentToEnable;
        [SerializeField]
        private SO_GenericEvent _EventToListenTo;

    #endregion

    #region Life Cycle

        private void OnEnable()
        {
            _EventToListenTo.AddListener(OnEvent);
        }

        private void OnDisable()
        {
            _EventToListenTo.RemoveListener(OnEvent);
        }

    #endregion

    #region Member Functions

        private void OnEvent(object data)
        {
            foreach (var component in _ComponentToEnable)
            {
                component.enabled = true;
            }
        }

    #endregion
}
