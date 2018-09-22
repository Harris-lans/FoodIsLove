using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour 
{
	#region Member Variables 

		[Header("Level Details")]
		[SerializeField]
		private string _LevelName;

		private SO_GridSelectEventHandler _GridSystemEventHandler;

	#endregion

	#region Member Functions

		public void SpawnPlayer()
		{
			
		}

	#endregion
}
