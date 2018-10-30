using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGameObject : MonoBehaviour 
{
	[SerializeField]
	private bool _ParentToGameObject = true;

	public void Spawn(GameObject objectToSpawn)
	{
		GameObject spawnedObject = Instantiate(objectToSpawn, transform.position, objectToSpawn.transform.rotation);
		
		if (_ParentToGameObject)
		{
			spawnedObject.transform.parent = transform;
		}
	}
}
