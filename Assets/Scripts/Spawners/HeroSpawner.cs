using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSpawner : MonoBehaviour 
{
	[SerializeField]
	private float _RadiusToCheckForBeforeSpawningHero = 2;

	public bool IsOccupied
	{
		get
		{
			bool isOccupied = false;
			Collider[] colliders = Physics.OverlapSphere(transform.position, _RadiusToCheckForBeforeSpawningHero);

			foreach(var collider in colliders)
			{
				if (collider.GetComponent<HeroController>() != null)
				{
					isOccupied = true;
					// Breaking from a for loop because the space is occupuied by a hero even if it is one hero
					break;
				}
			}

			return isOccupied;
		}
	}
}
