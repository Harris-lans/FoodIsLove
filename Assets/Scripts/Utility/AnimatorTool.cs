using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorTool : MonoBehaviour 
{
	#region Member Variables

		private Animator _Animator;

	#endregion

	#region Life Cycle

		private void Awake()
		{
			_Animator = GetComponent<Animator>();
		}

	#endregion

	#region Member Functions

		public void ChangeBool(BoolAnimation animationDetails)
		{
			_Animator.SetBool(animationDetails.BoolName, animationDetails.Value);
		}

		public void ChangeInteger(IntegerAnimation animationDetails)
		{
			_Animator.SetInteger(animationDetails.IntegerName, animationDetails.Value);
		}

		public void ChangeFloat(FloatAnimation animationDetails)
		{
			_Animator.SetFloat(animationDetails.FloatName, animationDetails.Value);
		}

		public void SetBoolToTrue(string boolName)
		{
			_Animator.SetBool(boolName, true);
		}	

		public void SetBoolToFalse(string boolName)
		{
			_Animator.SetBool(boolName, false);
		}

		public void Trigger(string triggerName)
		{
			_Animator.SetTrigger(triggerName);
		}

	#endregion

	#region Structs

		[System.Serializable]
		public struct BoolAnimation
		{
			public string BoolName;
			public bool Value; 
		}

		[System.Serializable]
		public struct FloatAnimation
		{
			public string FloatName;
			public float Value; 
		}

		[System.Serializable]
		public struct IntegerAnimation
		{
			public string IntegerName;
			public int Value; 
		}

	#endregion
}