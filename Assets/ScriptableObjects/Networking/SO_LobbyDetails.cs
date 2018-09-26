using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LobbyDetails", menuName="FoodIsLove/Networking/LobbyDetails")]
public class SO_LobbyDetails : ScriptableObject 
{
	[Header("Connection Details")]
	[HideInInspector]
	public string RoomName;
	[HideInInspector]
	public int MaximumPlayersAllowed;
	[HideInInspector]
	public int NumberOfPlayersInRoom;
	[HideInInspector]
	public int NumberOfPlayersReady;

	[Header("Default Connection Details")]
	[SerializeField]
	private string _DefaultRoomName;

	[Header("Judge Details")]
	public SO_Judge[] JudgeList;

	[Space,Header("Match Details")]
	[HideInInspector]
	public string LevelToLoad;
	[HideInInspector]
	public SO_Judge Judge;
	[HideInInspector]
	public SO_HeroData ChosenHero;
	[HideInInspector]
	public SO_MinionData[] ChosenMinions;
	[HideInInspector]
	public SO_Dish[] ChosenDishes;

	[Space,Header("Default Match Details")]
	[SerializeField]
	private string _DefaultLevelToLoad;
	[SerializeField]
	private SO_Judge _DefaultJudge;
	[SerializeField]
	private SO_HeroData _DefaultChosenHero;


	#region Member Functions

		public void Initialize(int maximumNumberOfPlayersInARoom)
		{
			// Intializing Connection Details
			RoomName = _DefaultRoomName;
			MaximumPlayersAllowed = maximumNumberOfPlayersInARoom;
			NumberOfPlayersInRoom = 0;
			NumberOfPlayersReady = 0;

			// Initializing Match Details
			LevelToLoad = _DefaultLevelToLoad;
			Judge = JudgeList[Random.Range(0, JudgeList.Length)];
			ChosenHero = _DefaultChosenHero;
			ChosenDishes = Judge.PreferredDishes;
		}

	#endregion
}
