using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpriteAnimator : MonoBehaviour 
{
	[SerializeField]
	private Transform _TransformToTrack;
	
	private Animator _Animator;
	private NavMeshAgent _NavMeshAgent;

	private void Start()
	{
		_NavMeshAgent = GetComponentInParent<NavMeshAgent>();
		_Animator = GetComponent<Animator>();

	    if (_NavMeshAgent != null)
	    {
	        StartCoroutine(AnimateHero());
	    }
	}

    private IEnumerator AnimateHero()
    {
        while (true)
        {
            int angle = (int)_TransformToTrack.localEulerAngles.y;

            if (angle > 180)
            {
                angle = angle - 360;
            }

            // Updating the the angle 
            _Animator.SetInteger("FacingAngle", angle);
            _Animator.SetFloat("Speed", _NavMeshAgent.velocity.magnitude);
            
            yield return null;
        }
    }
}
