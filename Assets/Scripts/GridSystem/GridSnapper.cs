using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSnapper : MonoBehaviour 
{
	#region Member Variables

	#endregion

	#region Member Functions

		private void Start()
		{
			// Snapping the position of the gameobject
			GridPosition currentGridPosition = GridSystem.Instance.GetGridPosition(transform.position);
			Vector3 cellCenter = GridSystem.Instance.GetActualCoordinates(currentGridPosition);
			cellCenter.y = transform.position.y;
			transform.position = cellCenter;
		}

	#endregion

}
