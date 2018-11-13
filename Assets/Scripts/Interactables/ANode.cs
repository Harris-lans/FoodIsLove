using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ANode : MonoBehaviour 
{
	public Transform SpotToStandIn;
	
	[Space, Header("Node Local Events")]
	[SerializeField]
	protected UnityEvent _ClickedOnNodeEvent;

	[Space, Header("Node Global Events")]
	[SerializeField]
	protected SO_GenericEvent _NodeClickedEvent;

	public virtual void OnClickedOnNode()
	{
		_ClickedOnNodeEvent.Invoke();
		_NodeClickedEvent.Invoke(this);
	}
}
