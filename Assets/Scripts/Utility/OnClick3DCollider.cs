using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class OnClick3DCollider : MonoBehaviour 
{
	[Header("Local Events")]
	[SerializeField]
	private UnityEvent _OnMouseUpEvent;
	[SerializeField]
	private UnityEvent _OnMouseDownEvent;

	private void OnMouseUp()
	{
		_OnMouseUpEvent.Invoke();
	}

	private void OnMouseDown() 
	{
		_OnMouseDownEvent.Invoke();
	}
}
