using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="FoodIsLove/GameState/MatchState", fileName = "MatchState")]
public class SO_MatchState : ScriptableObject 
{
	#region Member Variables

		[HideInInspector]
		public bool LocalPlayerControllerSpawned;
		[HideInInspector]
		public bool EnteredMatch = false;
		[HideInInspector]
		public bool MatchStarted = false;
		[HideInInspector]
		public int TimeLeft;
		[HideInInspector]
		public SO_Dish[] ExpectedDishes;
		[HideInInspector]
		public bool MatchOver;
		[HideInInspector]
		public bool WonTheMatch;
		[HideInInspector]
		public GameOverReason GameOverReason;
		[HideInInspector]
		public Dictionary<int, CookingPot> PlayerCookingPots;

	#endregion

	#region Member Functions

		public void Initialize(int totalMatchTime, SO_Dish[] expectedDishes)
		{
			LocalPlayerControllerSpawned = false;
			MatchStarted = false;
			MatchOver = false;
		    ExpectedDishes = expectedDishes;
			TimeLeft = totalMatchTime;
			WonTheMatch = false;
			GameOverReason = GameOverReason.GAME_NOT_OVER;
			PlayerCookingPots = new Dictionary<int, CookingPot>();
		}

		public void RegisterCookingPot(int playerViewId, CookingPot playersPot)
		{
			PlayerCookingPots[playerViewId] = playersPot;
		}

        public CookingPot GetCookingPot(int playerViewID)
        {
            return PlayerCookingPots[playerViewID];
        }

    #endregion
}
