using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteCameraFacer : MonoBehaviour 
{
	[SerializeField]
	private Transform _TransformToTrack;
	
	private Transform _MainCamera;

	private void Start()
	{
		_MainCamera = Camera.main.transform;
	}

	private void Update() 
	{
		// Maintaining the rotation of the sprite with in the game object
		Vector3 finalRotation = - 1 * _TransformToTrack.eulerAngles;

		// Making the sprite face the camera
		finalRotation.x = _MainCamera.localEulerAngles.x;

		transform.localEulerAngles = finalRotation;
	}
}
