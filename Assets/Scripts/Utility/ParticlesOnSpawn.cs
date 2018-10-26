using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesOnSpawn : MonoBehaviour 
{
	[SerializeField]
	private GameObject _SpawnParticles;

	void Start () 
	{
		var particles = Instantiate(_SpawnParticles, transform);
		particles.transform.position = Vector3.zero;
	}
}
