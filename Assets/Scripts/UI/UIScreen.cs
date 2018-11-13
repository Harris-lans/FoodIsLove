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
        
        [HideInInspector]
		private Animation _Animation;
		protected UIManager _UIManager;
        public UIScreenState State { get; private set; }

	#endregion

	#region Life Cycle

		protected virtual void Awake()
		{
			_Animation = GetComponent<Animation>();
			_UIManager = UIManager.Instance;
		}

	#endregion

	#region Member Functions

		public void ShowScreen()
		{
		    gameObject.SetActive(true);
		}

		public void HideScreen()
		{
            _Animation.Stop();
            gameObject.SetActive(false);
		}

        public IEnumerator PlayOutroAnimations()
        {
            State = UIScreenState.OUTRO;

            foreach (var animation in _AnimatonsToPlayOnTransitioningOut)
            {
                _Animation.PlayQueued(animation, QueueMode.PlayNow);
            }

            // Waiting for all the animations to end
            while (_Animation.isPlaying)
            {
                yield return null;
            }

            HideScreen();

            State = UIScreenState.HIDDEN;
        }

        public IEnumerator PlayIntroAnimations()
        {
            ShowScreen();

            State = UIScreenState.INTRO;

            foreach (var animation in _AnimatonsToPlayOnTransitioningIn)
            {
                _Animation.PlayQueued(animation, QueueMode.CompleteOthers);
            }

            // Waiting for all the animations to end
            while (_Animation.isPlaying)
            {
                yield return null;
            }

            State = UIScreenState.VISIBLE;
        }

        public bool IsAnimating
        {
            get 
            {
                return _Animation.isPlaying;
            }
        }

    #endregion
}

public enum UIScreenState : byte
{
    VISIBLE = 0,
    HIDDEN,
    INTRO,
    OUTRO
}