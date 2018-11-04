using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameMusicController : MonoBehaviour 
{	
	[Header("Music State Data")]
	[SerializeField]
	private string _MusicStateGroup;
	[SerializeField]
	private ValueEventPair[] _MusicValueEventPairs;

	[Space, Header("Required Data")]
	private SO_MatchState _MatchState;

	[Space, Header("Global Events")]
	[SerializeField]
	private SO_GenericEvent _IngredientAddedToCookingPotEvent;

	private Dictionary<int, string> _MusicValueEvents;

	#region Life Cycle

		private void Start()
		{
			PopulateDictionary();
			_IngredientAddedToCookingPotEvent.AddListener(OnIngredientAddedToCookingPot);
			AudioManager.SetState(_MusicStateGroup, _MusicValueEvents[1]);
		}

	#endregion

	#region Member Functions

		private void PopulateDictionary()
		{
			_MusicValueEvents = new Dictionary<int, string>();
			foreach(var pair in _MusicValueEventPairs)
			{
				_MusicValueEvents[pair.Value] = pair.EventName;
			}
		}

		private void OnIngredientAddedToCookingPot(object data)
		{
			// Since there are only two players for now
			CookingPot firstCookingPot = _MatchState.PlayerCookingPots.ElementAt(0).Value;
			CookingPot secondCookingPot = _MatchState.PlayerCookingPots.ElementAt(1).Value;
			int differenceInScore = (int)Mathf.Abs(firstCookingPot.NumberOfIngredientsInPlace - secondCookingPot.NumberOfIngredientsInPlace);
			differenceInScore = Mathf.Clamp(differenceInScore, 1, 4);
			AudioManager.SetState(_MusicStateGroup, _MusicValueEvents[differenceInScore]);
		}

	#endregion
}

[System.Serializable]
public struct ValueEventPair
{
	public int Value;
	public string EventName;
}
