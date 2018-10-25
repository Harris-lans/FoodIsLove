using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesOnSpawn : MonoBehaviour 
{
	[SerializeField]
	private GameObject _SpawnParticles;

	void Start () 
	{
		Instantiate(_SpawnParticles, transform.position, Quaternion.identity);	
	}
}
