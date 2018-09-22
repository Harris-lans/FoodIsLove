using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PSM;

[CreateAssetMenu(menuName = "PSM/GenericActions/SpawnParticles", fileName = "SpawnParticles")]
public class Action_SpawnParticles : SO_AAction 
{
	[SerializeField]
	private GameObject _ParticleEffectPrefab;

    public override void Act(PluggableStateMachine pluggableStateMachine)
    {
		// Instantiating the particle effects
        Instantiate(_ParticleEffectPrefab, pluggableStateMachine.gameObject.transform);
    }
}
