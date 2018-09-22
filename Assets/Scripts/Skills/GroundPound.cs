using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPound : ASkill_AOE 
{
    #region Member Variables

		[Header("Ground Pound Details")]
		[SerializeField]
		private float _DelayBeforeCast;
		[SerializeField]
        private float _StunTime;

    #endregion

    #region Life Cycle



    #endregion

    #region Member Functions

        public override void Cast(object data)
        {
			base.Cast(data);
			StartCoroutine(CastAfterDelay(_DelayBeforeCast));
        }

		private IEnumerator CastAfterDelay(float delayBeforeCast)
		{
			yield return new WaitForSeconds(delayBeforeCast);
			
			// Cast Spell
			Instantiate(_ParticleEffects, transform.position, Quaternion.identity);
		    Collider[] hitColliders = Physics.OverlapSphere(transform.position, _Radius);
		    foreach (var col in hitColliders)
		    {
				// Ignoring self
				if (col.gameObject == gameObject)
				{
					continue;
				}

			    var characterController = col.GetComponent<IStunnable>();
		        var character = col.GetComponent<IDamagable>();
				Rigidbody rigidBody = col.GetComponent<Rigidbody>();

			    if (character != null)
			    {
				    character.Damage(_Damage, gameObject);
			    }

		        if (characterController != null)
		        {
		            characterController.Stun(_StunTime, gameObject);

					if (rigidBody != null)
					{
						rigidBody.AddExplosionForce(_ForceToApply, transform.position, _Radius, 0.0f, ForceMode.Impulse);
					}
		        }
		    }
		}

    #endregion
}
