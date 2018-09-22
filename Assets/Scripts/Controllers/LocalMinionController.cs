using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using PSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalMinionController : MinionController 
{
    #region Member Variables

        [Header("Networking Details")]
        [SerializeField]
        private float _TimeIntervalBetweenSendingPositionUpdates = 0.2f;

        // State-machine variables
        private PluggableStateMachine _PluggableStateMachine;
        private SO_MinionStateMachineDetails _MinionStateMachineDetails;

        private GridPosition _PreviousGridPosition;
		private GridPosition _TargetGridPosition;
        private CookingStation _TargetCookingStation;
        private Coroutine _MinionNetworkPositionBroadcaster;

		// Networking Variables
		private RaiseEventOptions _RaiseEventOptions;
		private SendOptions _SendOptions;

    #endregion

    #region Life Cycle

        protected override void Awake()
        {
            base.Awake();
            
            // Getting the required state machine details
            _MinionStateMachineDetails = Resources.Load<SO_MinionStateMachineDetails>("Default_MinionStateMachineDetails");

            _PluggableStateMachine = new PluggableStateMachine(gameObject, _MinionStateMachineDetails.DefaultState);

			// Initializing Network variables
			_RaiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
			_SendOptions = new SendOptions { Reliability = false };

        }

        private void OnEnable()
        {
            StartCoroutine(SendCurrentPositions());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private void Update()
        {
            // Updating the state machine
            _PluggableStateMachine.OnUpdate();
        }

    #endregion

    #region  Member Functions

        #region Overriden Parent Functions

            public override void Cook(CookingStation targetCookingStation)
            {
                base.Cook(_TargetCookingStation);

                // Raising Net Event
                PhotonView cookingStation = _TargetCookingStation.GetComponent<PhotonView>();
                int[] data = { _PhotonView.ViewID, cookingStation.ViewID };
                PhotonNetwork.RaiseEvent((byte)NetworkedGameEvents.ON_MINION_COOK, data, _RaiseEventOptions, _SendOptions);
            }

            public override void MoveToTargetGrid(GridPosition targetGridPosition)
            {
                base.MoveToTargetGrid(_TargetGridPosition);
            }

        #endregion

        public void TargetCookingStation(GridPosition position, CookingStation cookingStation)
        {
            // Moving to cooking station only when not busy 
            if (IsTakingOrders)
            {
                // Change Pluggable State Machine's State to Move to cooking station
                _PluggableStateMachine.ChangeState(_MinionStateMachineDetails.MoveToCookingStationState);
                _TargetCookingStation = cookingStation;
                _TargetGridPosition = position;
            }
        }

	    public void MoveToHero()
        {
            _NavMover.SetDestination(HeroToFollow.PositionOnGrid);
        }

        public bool IsCloseToCookingStation(float minimumDistance)
        {
            if (Vector3.Distance(transform.position, _GridSystem.GetActualCoordinates(_TargetGridPosition)) <= minimumDistance)
            {
                return true;
            }

            return false;
        }

        public bool HasFinishedCooking()
        {
            IsTakingOrders = _TargetCookingStation.IsOnCooldown || _TargetCookingStation.StationInUse;
            return _TargetCookingStation.IsOnCooldown;
        }

    #endregion

    #region Co-Routines

        private IEnumerator SendCurrentPositions()
	    {
            while(true)
            {
                // Raising Net Event
                float[] data = { _PhotonView.ViewID, transform.position.x, transform.position.z };
                PhotonNetwork.RaiseEvent((byte)NetworkedGameEvents.ON_MINION_MOVED, data, _RaiseEventOptions, _SendOptions);

                yield return new WaitForSeconds(_TimeIntervalBetweenSendingPositionUpdates);
            }
        }

    #endregion
}
