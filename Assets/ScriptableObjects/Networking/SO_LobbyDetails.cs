using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "LobbyDetails", menuName="FoodIsLove/Networking/LobbyDetails")]
public class SO_LobbyDetails : ScriptableObject 
{
	[Header("Connection Details")]
	public int MaximumPlayersAllowed;
	public int NumberOfPlayersInRoom;
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
	public SO_HeroData OpponentHero;
	[HideInInspector]
	public SO_Dish[] ChosenDishes;

	[Space,Header("Default Match Details")]
	[SerializeField]
	private string _DefaultLevelToLoad;

	#region Member Functions

		public int[] Initialize(int judgeIndex = -1, int dishIndex = -1)
		{
			// Initializing Match Details
			LevelToLoad = _DefaultLevelToLoad;

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

		public void Reset() 
		{
			ChosenHero = null;
			OpponentHero = null;
		}

		public void InitializeLobbyConnectionDetails(int maximumNumberOfPlayersInARoom)
		{
			// Intializing Connection Details
			MaximumPlayersAllowed = maximumNumberOfPlayersInARoom;
			NumberOfPlayersInRoom = 0;
			NumberOfPlayersReady = 0;
			Reset();
		}

	#endregion
}
