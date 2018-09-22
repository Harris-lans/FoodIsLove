using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName="FoodIsLove/Level/LevelData")]
public class SO_LevelData : ScriptableObject
{
	[Header("Grid Data")]
	public SO_GridSelectEventHandler GridSelectEventHandler;
	
	[Space, Header("UI Data")]
	public SO_GenericEvent IngredientSelectEventHandler;
	public SO_GenericEvent CookingStationPopUpClickedEventHandler;

	[Space, Header("Match Details")]
	public SO_MatchState MatchState;

	[Space, Header("Lobby Details")]
	public SO_LobbyDetails LobbyDetails;

    [Space, Header("Ingredient Spawn Data")]
    public SO_IngredientSpawnData IngredientSpawnData;
}
