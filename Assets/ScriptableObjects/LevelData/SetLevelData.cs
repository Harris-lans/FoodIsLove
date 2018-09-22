using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLevelData : MonoBehaviour 
{
    #region Member Functions

    [Header("Grid Data")]
	[SerializeField]
	private SO_GridSelectEventHandler _GridSelectEventHandler;
	
	[Header("UI Data")]
	[SerializeField]
	private SO_GenericEvent _IngredientSelectEventHandler;
	[SerializeField]
	private SO_GenericEvent _CookingStationPopUpClickedEventHandler;

	[Space, Header("Match Details")]
	[SerializeField]
	private SO_MatchState _MatchState;

	[Space, Header("Lobby Details")]
	[SerializeField]
	private SO_LobbyDetails _LobbyDetails;

    [Space, Header("Ingredient Spawn Data")]
	[SerializeField]
    private SO_IngredientSpawnData _IngredientSpawnData;

    #endregion

    #region LifeCycle

    private void Awake()
	{
		SO_LevelData currentlevelData = Resources.Load<SO_LevelData>("CurrentLevelData");

		// Setting Level Data
		currentlevelData.GridSelectEventHandler = _GridSelectEventHandler; 
		currentlevelData.CookingStationPopUpClickedEventHandler = _CookingStationPopUpClickedEventHandler;
		currentlevelData.IngredientSelectEventHandler = _IngredientSelectEventHandler;
		currentlevelData.MatchState = _MatchState;
		currentlevelData.LobbyDetails = _LobbyDetails;
	    currentlevelData.IngredientSpawnData = _IngredientSpawnData;
	}

    #endregion

}
