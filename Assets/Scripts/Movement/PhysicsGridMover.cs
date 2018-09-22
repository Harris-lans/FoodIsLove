using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class PhysicsGridMover : MonoBehaviour, INavMover
{
	#region Member Variables

		[Space, Header("Pathfinding Details")]
		[SerializeField]
		private float _DelayBetweenPathCalculations;
		[SerializeField]
		private float _BaseMovementSpeed = 10;

		private Rigidbody _RigidBody;
		private NavMeshAgent _NavMeshAgent;
		private GridSystem _GridSystem;
		private Vector3 _Destination;
		private Vector3 _Direction;
		private NavMeshPath _NavMeshPath;

	#endregion

	#region Life Cycle

		private void Awake()
		{
			// Fetching Components
			_RigidBody = GetComponent<Rigidbody>();
			_NavMeshAgent = GetComponent<NavMeshAgent>();

			// Initializing 
			_GridSystem = GridSystem.Instance;
			_NavMeshAgent.velocity = Vector3.zero;
			_NavMeshPath = new NavMeshPath();
			_Destination = transform.position;
		}

		private void Start()
		{

		}

		private void Update()
		{
			// Calculating path to destintion
			_NavMeshAgent.CalculatePath(_Destination, _NavMeshPath);

			for (int i = 0; i < _NavMeshPath.corners.Length; ++i)
			{
				if (Vector3.Distance (transform.position, _NavMeshPath.corners[i]) > 0.5f)
				{
					_Direction = _NavMeshPath.corners[i] - transform.position;
					return;
				}
			}
		}

		private void FixedUpdate()
		{
			_RigidBody.AddForce(_Direction.normalized * _BaseMovementSpeed * Time.deltaTime, ForceMode.Acceleration);
		}

	#endregion

	#region Member Variables

		public void SetDestination(GridPosition destination)
		{
			_Destination = _GridSystem.GetActualCoordinates(destination);
		}

		public bool ReachedDestination()
		{
			return false;
		}

		public void StopMoving()
		{

		}

    #endregion
}
