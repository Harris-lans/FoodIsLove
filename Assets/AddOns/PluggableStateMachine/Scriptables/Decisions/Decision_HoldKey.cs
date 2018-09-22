using UnityEngine;
using PSM;

[CreateAssetMenu(menuName="PSM/Decisions/HoldKey", fileName = "Hold_")]
public class Decision_HoldKey : SO_ADecision
{
	[SerializeField]
	private KeyCode _KeyToDetect;

    public override bool Decide(PluggableStateMachine pluggableStateMachine)
    {
        return Input.GetKey(_KeyToDetect);
    }
}
