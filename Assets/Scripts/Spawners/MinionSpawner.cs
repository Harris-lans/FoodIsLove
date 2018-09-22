using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class MinionSpawner : ANode 
{
	#region Member Variables

        [Header("Spawner Details")] 
        [SerializeField]
        private float _TimeBeforeSpawningNextIngredient = 10.0f;

        private float _TimerTime;
        private SO_IngredientSpawnData _IngredientSpawnData;
        private Coroutine _IngredientSpawner;
        private bool _CanSpawn;
        private SO_Tag _SpawnedIngredient;

    #endregion

    #region Life Cycle

        private void Start()
        {
            // Loading Resources
            SO_LevelData levelData = Resources.Load<SO_LevelData>("CurrentLevelData");

            _IngredientSpawnData = levelData.IngredientSpawnData;

            // Initializing variables
            _CanSpawn = true;
        }

        private void OnEnable()
        {
            // Only the master client is allowed to spawn ingredients
            if (PhotonNetwork.IsMasterClient)
            {
                _IngredientSpawner = StartCoroutine(SpawnIngredients());
            }
        }

        private void OnDisable()
        {
            if (_IngredientSpawner == null)
            {
                StopCoroutine(_IngredientSpawner);
            }
        }

        public void PickedUpIngredient()
        {
            // Starting spawn timer
            StartCoroutine(SpawnTimer());
            _SpawnedIngredient = null;
        }


    #endregion

    #region Member Functions

        private IEnumerator SpawnIngredients()
        {
            while (true)
            {
                if (_CanSpawn && _SpawnedIngredient == null)
                {
                    _SpawnedIngredient = _IngredientSpawnData.ChooseIngredientToSpawn();
                    PhotonNetwork.Instantiate(_SpawnedIngredient.name.Replace("Tag_", ""), transform.position, Quaternion.identity);
                }

                yield return null;
            }
        }

        private IEnumerator SpawnTimer()
        {
            _CanSpawn = false;

            _TimerTime = _TimeBeforeSpawningNextIngredient;
            while (_TimerTime > 0)
            {
                yield return new WaitForSeconds(0.1f);
                _TimerTime -= 0.1f;
            }

            _CanSpawn = true;
        }

    #endregion

    #region Properties

        private float SpawnTimerFraction
        {
            get
            {
                return _TimerTime / _TimeBeforeSpawningNextIngredient;
            }
        }

    #endregion
}
