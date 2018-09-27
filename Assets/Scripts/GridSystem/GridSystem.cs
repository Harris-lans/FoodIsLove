using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GridSystem : SingletonBehaviour<GridSystem>
{
	#region Global Variables

		[Header("Grid Properties")]
		[Tooltip("The number of cells along the X - Axis")]
		[SerializeField]
		private int _NumberOfCellsX;
		[Tooltip("The number of cells along the Z - Axis")]
		[SerializeField]
		private int _NumberOfCellsZ;
		[Tooltip("This property is used to adjust the size of the cells in the grid. (Note: The cells are always square)")]
		[SerializeField]
		private float _GridGranularity;
		[Tooltip("This tells the Grid System which layer of objects it should check when it detects a click")]
		[SerializeField]
		private LayerMask _GridTouchLayerMask;
		[SerializeField]
		private float _TimeBeforeUpdatingGridPosition = 1.0f;

		[Space, Header("Grid Debug Tools")]
		[SerializeField]
		private bool _DebugMode = false;
		[SerializeField]
		private Color _DebugColor = Color.blue;
		[SerializeField]
		private Color _OccupiedCellsColor = Color.red;
        
        [Header("Grid Events")]
        public UnityEvent GridSelectEvent;
		[SerializeField]
		private SO_GridSelectEventHandler _GridSelectEventHandler;


		public GridProp[,] GridData;

	#endregion

	#region Life Cycle

    	protected override void SingletonAwake()
    	{
			// Initializing Grid Data
			GridData = new GridProp[_NumberOfCellsX, _NumberOfCellsZ];

			// Detecting Obstacles
			DetectObstacles();

			// Disabling multitouch detection
			Input.multiTouchEnabled = false;

			// Checking the platform and optimizing the inputs accordingly
			if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
			{
				StartCoroutine(DetectMouseClicks());
			}
			else if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
			{
				StartCoroutine(DetectTouch());
			}

			// Updating GridProps
			GridProp.DynamicGridPropsList = new HashSet<GridProp>();
			StartCoroutine(UpdateGridProps());
    	}

        protected override void SingletonOnDestroy()
		{
			StopAllCoroutines();
		} 

		private void OnDrawGizmos()
		{
			if (_DebugMode)
			{
				for (int i = 0; i < _NumberOfCellsX; i++)
				{
					for (int j = 0; j < _NumberOfCellsZ; j++)
					{
						Vector3 drawPosition = transform.position + (transform.right * i * _GridGranularity) + (transform.right * _GridGranularity * 0.5f) + (transform.forward * j * _GridGranularity) + (transform.forward * _GridGranularity * 0.5f);
						Vector3 cubeSize = new Vector3(_GridGranularity, 0.05f, _GridGranularity);

						Gizmos.color = _DebugColor;
						if (GridData != null && GridData[i, j] != null)
						{
							Gizmos.color = _OccupiedCellsColor;
						}
						Gizmos.DrawWireCube((drawPosition + transform.up * 0.05f), cubeSize);
					}			
				}
			}
		}

    #endregion

    #region Member Functions

        public GridPosition GetGridPosition(Vector3 worldSpacePosition)
        {
            GridPosition gridPosition = new GridPosition();
            // NOTE: This grid system ignores y-axis
            // Converting the world space position passed to grid co-ordinates
            gridPosition.X = (byte)(((worldSpacePosition.x - transform.position.x)) / GridGranularity);
            gridPosition.Z = (byte)(((worldSpacePosition.z - transform.position.z)) / GridGranularity);

            return gridPosition;
        }

        public Vector3 GetActualCoordinates(GridPosition gridPosition)
        {
            Vector3 worldSpacePosition = new Vector3();

            // Converting grid co-ordinates to world space position 
            worldSpacePosition.x = (gridPosition.X * GridGranularity) + (GridGranularity * 0.5f) + transform.position.x;
            worldSpacePosition.y = transform.position.y;
            worldSpacePosition.z = (gridPosition.Z * GridGranularity) + (GridGranularity * 0.5f) + transform.position.z;

            return worldSpacePosition;
        }


        private void DetectObstacles()
		{
			for (int i = 0; i < _NumberOfCellsX; i++)
			{
				for (int j = 0; j < _NumberOfCellsZ; j++)
				{
					Vector3 drawPosition = transform.position + (transform.right * i * _GridGranularity) + (transform.right * _GridGranularity * 0.5f) + (transform.forward * j * _GridGranularity) + (transform.forward * _GridGranularity * 0.5f);
					Vector3 cubeSize = new Vector3(_GridGranularity * 0.25f, 1, _GridGranularity * 0.25f);
					//Collider[] objectsInSpace = Physics.OverlapBox((drawPosition + transform.up * 2.0f), cubeSize);
					RaycastHit[] objectsInSpace = Physics.RaycastAll(drawPosition, Vector3.up, 30);
					foreach (var item in objectsInSpace)
					{
						GridProp gridProperty = item.collider.GetComponent<GridProp>();
						
						if (gridProperty != null)
						{
							GridData[i, j] = gridProperty;
							break;
						}
					}
				}			
			}
		}

		private IEnumerator UpdateGridProps()
		{
			// TODO: Optimize the updating of grid positions further
			
			while (true)
			{
				foreach (var gridProp in GridProp.DynamicGridPropsList)
				{
					GridPosition currentGridPosition = GetGridPosition(gridProp.transform.position);
					if (gridProp.PositionOnGrid != currentGridPosition)
					{
						// Changing the grid position of the gameobject with the grid property
						GridData[gridProp.PositionOnGrid.X, gridProp.PositionOnGrid.Z] = null;
						GridData[currentGridPosition.X, currentGridPosition.Z] = gridProp;
						gridProp.PositionOnGrid = currentGridPosition;
					}
				}
				yield return new WaitForSeconds(_TimeBeforeUpdatingGridPosition);
			}
		} 

		private IEnumerator DetectMouseClicks()
		{
			while (true)
			{
				if(Input.GetMouseButtonDown(0))
				{
					CheckIfScreenSpacePointInGrid(Input.mousePosition);
				}

				yield return null;
			}
		}

		private IEnumerator DetectTouch()
		{
			while (true)
			{
				// Multitouch support
				foreach (Touch touch in Input.touches)
				{
					// Only considering inputs when the player takes his finger from the screen
					if (touch.phase == TouchPhase.Ended)
					{
						CheckIfScreenSpacePointInGrid(touch.position);
					}
				}

				yield return null;
			}
		}

		private void CheckIfScreenSpacePointInGrid(Vector3 point)
		{
			Ray ray = Camera.main.ScreenPointToRay(point);
			RaycastHit hit;


			if (Physics.Raycast(ray, out hit, 100.0f, _GridTouchLayerMask))
			{
				Debug.LogFormat("Hit {0}", hit.collider.name);
				if (hit.collider.CompareTag("Ground"))
				{ 
					GridPosition touchPositionOnGrid = GetGridPosition(hit.point);
					
					// Checking if touch position is outside the grids 
					if ((touchPositionOnGrid.X < 0 || touchPositionOnGrid.X > _NumberOfCellsX) || ((touchPositionOnGrid.Z < 0 || touchPositionOnGrid.Z > _NumberOfCellsZ)))
					{
						return;
					}

					GridProp selectedGridProperty = GridData[touchPositionOnGrid.X, touchPositionOnGrid.Z];
                    _GridSelectEventHandler.InvokeEvent(touchPositionOnGrid, selectedGridProperty);
                    GridSelectEvent.Invoke();
                }
			}
		}

	#endregion

	#region Properties

		public int NumberOfCellsX
		{
			get
			{
				return _NumberOfCellsX;
			}
		}

		public int NumberOfCellsZ
		{
			get
			{
				return _NumberOfCellsZ;
			}
		}

		public float GridGranularity
		{
			get 
			{
				return _GridGranularity;
			}
		}

	#endregion
}