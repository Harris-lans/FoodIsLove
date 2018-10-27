using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerIdentificationRing : MonoBehaviour 
{
	#region Member Variables
	
		[Header("Ring Details")]
		[SerializeField]
		private GameObject _StationaryPlayerRingPrefab;
		[SerializeField]
		private GameObject _LocalPlayerRingPrefab;
		[SerializeField]
		private GameObject _RemotePlayerRingPrefab;

		private NavMeshAgent _HeroNavMesh;
		private Renderer _Renderer;
		private GameObject _MobilePlayerRing;
		private GameObject _StationaryPlayerRing;

	#endregion

	#region Life Cycle

		private void Start() 
		{
			_HeroNavMesh = GetComponentInParent<NavMeshAgent>();
			_Renderer = GetComponent<Renderer>();

			// Spawning the stationary ring
			_StationaryPlayerRing = Instantiate(_StationaryPlayerRingPrefab, transform.position, _StationaryPlayerRingPrefab.transform.rotation);
			_StationaryPlayerRing.transform.parent = transform;
		}

		private void LateUpdate() 
		{
			if (_HeroNavMesh.velocity.sqrMagnitude <= 0.1f)
			{
				_MobilePlayerRing.SetActive(false);
				_StationaryPlayerRing.SetActive(true);
			}
			else
			{
				_MobilePlayerRing.SetActive(true);
				_StationaryPlayerRing.SetActive(false);
			}
		}

	#endregion

	#region Member Functions

		public void Initialize(bool isLocal)
		{
			if (isLocal)
			{
				_MobilePlayerRing = Instantiate(_LocalPlayerRingPrefab, transform.position, _LocalPlayerRingPrefab.transform.rotation);
			}
			else
			{
				_MobilePlayerRing = Instantiate(_RemotePlayerRingPrefab, transform.position, _RemotePlayerRingPrefab.transform.rotation);
			}
			_MobilePlayerRing.transform.parent = transform;
		}

	#endregion
}

public enum PlayerTypes : byte
{
	LOCAL = 0,
	REMOTE
}