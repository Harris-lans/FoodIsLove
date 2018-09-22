using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethodVector3 
{

	public static Vector3 Hermite(this Vector3 origin, Vector3 final, float factor)
	{
		return new Vector3 (
			 origin.x.Hermite(final.x, factor)
			,origin.y.Hermite(final.y, factor)
			,origin.z.Hermite(final.z, factor));
	}

	public static float Hermite(this float from, float to, float percent)
	{
		float hermite = (3 * Mathf.Pow(percent, 2)) - (2 * Mathf.Pow(percent, 3));
		return Mathf.Lerp(from, to, hermite);
	}
}
