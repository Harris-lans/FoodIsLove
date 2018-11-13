using UnityEngine;

[CreateAssetMenu(fileName = "NetMatchDetails", menuName="FoodIsLove/GameData/NetMatchDetails")]
public class SO_NetMatchDetails : ScriptableObject 
{
	public Color LocalPlayerColor;
	public Color RemotePlayerColor;
	public int GameTime;
}
