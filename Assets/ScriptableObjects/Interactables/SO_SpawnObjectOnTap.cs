using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnObjectOnTap", menuName = "FoodIsLove/TerrainReaction/SpawnObjectOnTap")]
public class SO_SpawnObjectOnTap : ATerrainReaction
{
	[SerializeField]
	private GameObject _ObjectPrefab;

    public override void React(Vector3 positionOfTouch, Vector3 objectUpVector)
    {
        GameObject spawnedObject = Instantiate(_ObjectPrefab, positionOfTouch, Quaternion.identity);
		spawnedObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, objectUpVector);
    }
}
