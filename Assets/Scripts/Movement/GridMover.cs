using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class GridMover : MonoBehaviour, INavMover 
{
	#region Global Variables

        private GridSystem _GridSystem;
		private NavMeshAgent _NavMeshAgent;

	#endregion

	private void Start () 
	{
		_NavMeshAgent = GetComponent<NavMeshAgent>();
	    _GridSystem = GridSystem.Instance;
	}
	
	public void SetDestination(GridPosition destination)
    {
		// Setting the new destination
		_NavMeshAgent.isStopped = false;
		_NavMeshAgent.SetDestination(_GridSystem.GetActualCoordinates(destination));
    }

    public bool ReachedDestination()
    {
        return _NavMeshAgent.isStopped;
    }

	public void StopMoving()
	{
		_NavMeshAgent.isStopped = true;
	}

    public void ContinueMoving()
    {
        _NavMeshAgent.isStopped = false;
    }
}
