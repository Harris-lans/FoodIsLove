using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroUISlot : MonoBehaviour 
{
	#region Member Variables

		[SerializeField]
		private SO_UIHeroSlot _UISlotData;

		private Slider _HealthBar;
		private Text _Name;

	#endregion 

	#region Life Cycle

		private void Start()
		{
			_Name.text = _UISlotData.Name;
		}

		private void Update()
		{
			//_HealthBar.value = _Health.GetHealthFraction();
		}

	#endregion

	#region Member Functions

	#endregion
}
