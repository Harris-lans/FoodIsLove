using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class SkillCaster : MonoBehaviour 
{
	[SerializeField]
	private Dictionary<ASkill.SkillTag, ASkill> _HeroSkills;

	private void Start()
	{
        // Detect and assign skills
		ASkill[] detectedSkills = GetComponents<ASkill>();

	    foreach (var skill in detectedSkills)
	    {
	        if (!_HeroSkills.ContainsKey(skill.Tag))
	        {
	            _HeroSkills[skill.Tag] = skill;
	        }
	    }

	}

    private void CastSkill(ASkill.SkillTag skillType)
    {
        if (_HeroSkills.ContainsKey(skillType) && _HeroSkills[skillType].State == ASkill.SkillStates.AVAILABLE)
        {
            _HeroSkills[skillType].Cast(null);
        }
    }

}
