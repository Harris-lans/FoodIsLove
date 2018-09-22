using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour 
{
	[SerializeField]
	private float _TimeBeforeDestroy = 5;
	[SerializeField]
	private GameObject _mParticle;

	// Use this for initialization
	void Start () 
	{
		Destroy(this.gameObject, _TimeBeforeDestroy);	
	}

	private void OnDestroy()
	{
		if(_mParticle != null)
		{
			GameObject _particle = Instantiate(_mParticle, transform.position, transform.rotation);
			Destroy(_particle, 2.0f);
		}
	}
}
