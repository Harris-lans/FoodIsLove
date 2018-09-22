using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PSM;
using UnityEngine.AI;
using Photon.Pun;

[RequireComponent(typeof(INavMover))]
public class MinionController : MonoBehaviour, IStunnable
{
    #region Member Variables
        
        [Header("Minion State Details")]
        public bool IsTakingOrders;

		[Header("Minion Details:")]
		[SerializeField]
		public GridProp HeroToFollow;

		[HideInInspector]
		public PhotonView OwnerPlayerController;

        protected GridSystem _GridSystem;
		protected IngredientMinion _IngredientMinion;
		protected INavMover _NavMover;
		protected NavMeshAgent _NavMeshAgent;
        protected PhotonView _PhotonView;
	
	#endregion

	#region Life Cycle

		protected virtual void Awake()
		{
			_IngredientMinion = GetComponent<IngredientMinion>();
			_NavMover = GetComponent<INavMover>();
		    _NavMeshAgent = GetComponent<NavMeshAgent>();
		    _PhotonView = GetComponent<PhotonView>();

            // Initializing Variables
            IsTakingOrders = true;
		    _GridSystem = GridSystem.Instance;
		}

	#endregion

	#region Member Functions

		public void Initialize(PhotonView ownerPlayerController, GridProp heroToFollow)
		{
			OwnerPlayerController = ownerPlayerController;
			HeroToFollow = heroToFollow;
		}

		public virtual void MoveToTargetGrid(GridPosition targetGridPosition)
		{
			_NavMover.SetDestination(targetGridPosition);
		}

		public virtual void Cook(CookingStation targetCookingStation)
		{
			if (targetCookingStation.IsAvailable)
			{
                IsTakingOrders = false;
                targetCookingStation.Use(_IngredientMinion);
			}
		}

        public virtual void Stun(float stunTime, GameObject attackOwner)
        {
			// Ignoring stun if the hero causing it is the master hero
            if (attackOwner.gameObject == HeroToFollow.gameObject)
			{
				return;
			}

			Debug.Log("Stunning Minion");
			_NavMeshAgent.enabled = false;
        }

        public IEnumerator StunTimer()
        {
            yield return null;
        }

    #endregion
}
