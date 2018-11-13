using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ATerrainReaction : ScriptableObject
{
	public abstract void React(Vector3 positionOfTouch, Vector3 objectUpVector);
}
