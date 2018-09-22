using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASkill_Ranged : ASkill 
{
	#region Member Variables

		[SerializeField]
		private float _Range;

	#endregion

	#region  Member Functions

		public override void Cast(object data)
		{
			Debug.Log("Something");
		}

    #endregion
}
