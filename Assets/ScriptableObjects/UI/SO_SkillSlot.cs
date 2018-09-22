using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "UISkillSlot", menuName = "FoodIsLove/UI/UISkillSlot", order = 0)]
public class SO_SkillSlot : ScriptableObject 
{
	public Sprite Thumbnail;
    public string Name;
    public ASkill.SkillTag TypeOfSkillToHouse;
	public ASkill Skill;

    public void Initialize(ASkill skill)
    {
        Skill = skill;
        Thumbnail = skill.Thumbnail;
        Name = skill.Name;
        TypeOfSkillToHouse = skill.Tag;
    }
}
