using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleColorChanger : MonoBehaviour
{
    private ParticleSystem _ParticleSystem;

	[Space, Header("Colors to change to")]
	[SerializeField]
	private Color _InitialColor;
	[SerializeField]
	private Color _MidColor;
	[SerializeField]
	private Color _EndColor;
	[SerializeField]
	private float _SecondBurstTimer = 2.0f;
	[SerializeField]
	private float _ThirdBurstTimer = 3.0f;

    private void Awake()
    {
        _ParticleSystem = GetComponent<ParticleSystem>();

    }

	void Start()
	{
		// var numberOfBursts = _ParticleSystem.emission.burstCount;
		// for (int i = 0; i < numberOfBursts; ++i)
		// {
		// 	var burst = _ParticleSystem.emission.GetBurst(i);
		// }
		StartCoroutine(ChangeBurstColors());
	}

    private IEnumerator ChangeBurstColors()
    {
		_ParticleSystem.startColor = _InitialColor;
		yield return new WaitForSeconds(_SecondBurstTimer);
		_ParticleSystem.startColor = _MidColor;
		yield return new WaitForSeconds(_ThirdBurstTimer);
		_ParticleSystem.startColor = _EndColor;
    }

}
