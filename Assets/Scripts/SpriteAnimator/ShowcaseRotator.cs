using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowcaseRotator : MonoBehaviour 
{
	[SerializeField]
	private float _RotationSpeed = 0.01f;

	void Update () 
	{
		Vector3 angle = transform.localEulerAngles;
		angle.y += _RotationSpeed;
		transform.localEulerAngles =  angle;	
	}
}
