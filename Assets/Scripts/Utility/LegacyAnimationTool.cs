using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegacyAnimationTool : MonoBehaviour 
{
	private Animation _Animation;

	private void Awake()
	{
		_Animation = GetComponent<Animation>();
	}

	public void QueueAnimation(string animationToPlay)
	{
		_Animation.Play(animationToPlay, AnimationPlayMode.Queue);
	}

	public void MixAnimation(string animationToPlay)
	{
		_Animation.Play(animationToPlay, AnimationPlayMode.Mix);
	}

	public void PlayAnimation(string animationToPlay)
	{
		Debug.Log("Playing Legacy Animation");
		_Animation.Play(animationToPlay);
	}
}
