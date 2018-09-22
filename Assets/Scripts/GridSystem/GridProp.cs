using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GridProp : MonoBehaviour 
{
	#region Global Variables

		public static HashSet<GridProp> DynamicGridPropsList;

		public GridPosition PositionOnGrid;	
		private SO_LevelData _LevelData;

		[SerializeField]
		private bool _IsStatic = false;

	#endregion
	
	#region Life Cycle

		private void Start()
		{
			if (!_IsStatic)
			{	
				if (DynamicGridPropsList != null)
				{
					DynamicGridPropsList.Add(this);
				}
			}
		}

		private void OnDestroy()
		{
			if (DynamicGridPropsList != null)
			{
				DynamicGridPropsList.Remove(this);
			}
			StopAllCoroutines();
		}

	#endregion

	#region Member Functions

		

	#endregion
}
