using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INavMover
{
	void SetDestination(Vector3 destination);
	bool ReachedDestination();
	void StopMoving();
	void ContinueMoving();
}
