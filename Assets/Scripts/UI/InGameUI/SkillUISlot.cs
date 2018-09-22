using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUISlot : MonoBehaviour 
{
	[SerializeField]
	private SO_SkillSlot _UISlotData;

	[SerializeField]
	private ASkill _Skill;

	private Slider _CoolDownBar;
	private Text _Name;

	#region Life Cycle

		private void Start()
		{

			// Storing the reference to the skill
			_Skill = _UISlotData.Skill;
			_CoolDownBar = GetComponentInChildren<Slider>();
			_Name = GetComponentInChildren<Text>();

			_Name.text = _Skill.Name;
		}

		private void LateUpdate() 
		{
			_CoolDownBar.value = _Skill.CoolDownFraction;
		}

	#endregion

	#region Member Functions

		public void OnClick()
		{
			
		}

	#endregion
}
