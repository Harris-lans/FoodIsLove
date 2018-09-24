using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INavMover
{
	void SetDestination(GridPosition destination);
	bool ReachedDestination();
	void StopMoving();
	void ContinueMoving();
}
