using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SO_SkillSelectEventHandler", menuName = "FoodIsLove/UI/SO_SkillSelectEventHandler", order = 0)]
public class SO_SkillSelectEventHandler : ScriptableObject 
{

    private event SkillSelectAction _SkillSelectEvent;

	private void Awake()
	{
		_SkillSelectEvent = null;
	}

	public void SubscribeToSkillSelectEvent(SkillSelectAction action)
	{
		_SkillSelectEvent += action;
	}

	public void InvokeEvent(ASkill SelectedSkill)
	{
		if (_SkillSelectEvent != null)
		{
			_SkillSelectEvent.Invoke(SelectedSkill);
		}
	}

    public delegate void SkillSelectAction(ASkill SelectedSkill);
}
