using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class GridMover : MonoBehaviour, INavMover 
{
	#region Global Variables

        private GridSystem _GridSystem;
		private NavMeshAgent _NavMeshAgent;

		public UnityEvent MovementStartedEvent;
		public UnityEvent MovementStoppedEvent;

	#endregion

	private void Start () 
	{
		_NavMeshAgent = GetComponent<NavMeshAgent>();
	    _GridSystem = GridSystem.Instance;
	}
	
	public void SetDestination(Vector3 destination)
    {
		// Setting the new destination
		_NavMeshAgent.isStopped = false;
		_NavMeshAgent.SetDestination(destination);
		MovementStartedEvent.Invoke();
    }

    public bool ReachedDestination()
    {
        return _NavMeshAgent.isStopped;
    }

	public void StopMoving()
	{
		_NavMeshAgent.isStopped = true;
		MovementStoppedEvent.Invoke();
	}

    public void ContinueMoving()
    {
        _NavMeshAgent.isStopped = false;
    }
}
