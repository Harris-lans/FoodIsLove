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
	private string _EventToInvoke;
	[SerializeField]
	private ValueEventPair[] _MusicValueEventPairs;

	[Space, Header("Required Data")]
	[SerializeField]
	private SO_MatchState _MatchState;

	[Space, Header("Global Events")]
	[SerializeField]
	private SO_GenericEvent _IngredientAddedToCookingPotEvent;

	private CookingPot _FirstCookingPot;
	private CookingPot _SecondCookingPot;

	private Dictionary<int, string> _MusicValueEvents;

	#region Life Cycle

		private void Start()
		{
			PopulateDictionary();
			_IngredientAddedToCookingPotEvent.AddListener(OnIngredientAddedToCookingPot);
			AudioManager.SetState(_MusicStateGroup, _MusicValueEvents[1]);
			StartCoroutine(CollectCookingPot());
		}

	#endregion

	#region Member Functions

		private IEnumerator CollectCookingPot()
		{
			while (_FirstCookingPot == null)
			{
				if (_MatchState.PlayerCookingPots.Count > 0)
				{
					_FirstCookingPot = _MatchState.PlayerCookingPots.ElementAt(0).Value;
				}
				yield return null;
			}
		}

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
			if (_FirstCookingPot != null && _MusicValueEvents.ContainsKey(_FirstCookingPot.NumberOfIngredientsInPlace))
			{
				AudioManager.SetState(_MusicStateGroup, _MusicValueEvents[_FirstCookingPot.NumberOfIngredientsInPlace]);
			}
		}

	#endregion
}

[System.Serializable]
public struct ValueEventPair
{
	public int Value;
	public string EventName;
}
