using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
	public SO_Dish[] ChosenDishes;

	[Space,Header("Default Match Details")]
	[SerializeField]
	private string _DefaultLevelToLoad;
	[SerializeField]
	private SO_Judge _DefaultJudge;
	[SerializeField]
	private SO_HeroData _DefaultChosenHero;


	#region Member Functions

		public int[] Initialize(int maximumNumberOfPlayersInARoom, int judgeIndex = -1, int dishIndex = -1)
		{
			// Intializing Connection Details
			RoomName = _DefaultRoomName;
			MaximumPlayersAllowed = maximumNumberOfPlayersInARoom;
			NumberOfPlayersInRoom = 0;
			NumberOfPlayersReady = 0;

			// Initializing Match Details
			LevelToLoad = _DefaultLevelToLoad;

			if (ChosenHero == null)
			{
				ChosenHero = _DefaultChosenHero;
			}

			Judge = JudgeList[Random.Range(0, JudgeList.Length)];
		    if (judgeIndex > -1)
		    {
		        Judge = JudgeList[judgeIndex];
		    }

			ChosenDishes = new SO_Dish[1];
		    ChosenDishes[0] = Judge.ChosenDish;

		    if (dishIndex > -1)
		    {
		        ChosenDishes[0] = Judge.PreferredDishes[dishIndex];
		    }

			int[] indices = {Array.IndexOf(JudgeList, Judge), Array.IndexOf(Judge.PreferredDishes, ChosenDishes[0])};
			return indices;
		}

	#endregion
}
