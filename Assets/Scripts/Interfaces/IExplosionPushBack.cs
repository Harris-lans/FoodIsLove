using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IExplosionPushBack 
{
	void ActivatePushBack();
	IEnumerator PhysicsTimer(float timeBeforeDeactivatingPhysics);
}
