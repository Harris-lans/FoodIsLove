using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FoodIsLove/UI/GameUIData", fileName = "GameUIData")]
public class SO_GameUIData : ScriptableObject 
{
	[Header("Character Slots")]
	public SO_UIHeroSlot HeroSlot;
	public SO_UIMinionSlot MinionSlot1;
	public SO_UIMinionSlot MinionSlot2;
	public SO_UIMinionSlot MinionSlot3;
	public SO_UIMinionSlot MinionSlot4;

	[Header("Skill Slots")]
	public SO_SkillSlot PrimarySkillSlot;
	public SO_SkillSlot SecondarySkillSlot;
	public SO_SkillSlot PassiveSkillSlot;
}
