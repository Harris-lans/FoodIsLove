using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScreen : MonoBehaviour 
{
	#region Member Variables
		
		[Header("Screen Details")]
		[SerializeField]
		public SO_Tag UIScreenTag;

		[Header("Animations")]
		[SerializeField]
		private string[] _AnimatonsToPlayOnTransitioningIn;
		[SerializeField]
		private string[] _AnimatonsToPlayOnTransitioningOut;

		protected UIManager _UIManager;
		protected Animation _Animation;

	#endregion

	#region Life Cycle

		protected virtual void Awake()
		{
			_UIManager = UIManager.Instance;
			_Animation = GetComponent<Animation>();
		}

	#endregion

	#region Member Functions

		public void ShowScreen()
		{
			gameObject.SetActive(true);
			StartCoroutine(PlayBeginAnimations());
		}

		public void HideScreen()
		{
			StartCoroutine(PlayEndAnimations());
		}

		private IEnumerator PlayBeginAnimations()
		{
			foreach (var animation in _AnimatonsToPlayOnTransitioningIn)
			{
				_Animation.PlayQueued(animation, QueueMode.CompleteOthers);
			}

			// Waiting for all the animations to end
			while(_Animation.isPlaying)
			{
				yield return null;
			}
		}

		public IEnumerator PlayEndAnimations()
		{
			// Queueing all the animations
			foreach (var animation in _AnimatonsToPlayOnTransitioningOut)
			{
				_Animation.PlayQueued(animation, QueueMode.CompleteOthers);
			}

			// Waiting for all the animations to end
			while(_Animation.isPlaying)
			{
				yield return null;
			}

			// Then Hiding the screen
			gameObject.SetActive(false);
		}

	#endregion
}