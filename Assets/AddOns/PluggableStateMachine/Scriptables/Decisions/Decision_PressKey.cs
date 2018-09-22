using UnityEngine;
using PSM;

[CreateAssetMenu(menuName="PSM/Decisions/PressKey", fileName = "Press_")]
public class Decision_PressKey : SO_ADecision
{
	[SerializeField]
	private KeyCode _KeyToDetect;

    public override bool Decide(PluggableStateMachine pluggableStateMachine)
    {
        return Input.GetKeyDown(_KeyToDetect);
    }
}
