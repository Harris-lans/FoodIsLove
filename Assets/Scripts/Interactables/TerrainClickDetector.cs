using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainClickDetector : MonoBehaviour 
{
	#region Member Variables

		[SerializeField]
		private TerrainReactionData[] _TerrainReactionList;
		[SerializeField]
		private LayerMask _RaycastLayerMask;

		private Dictionary<string, List<ATerrainReaction>> _TerrainReactionData;
		private Camera _MainCamera;

	#endregion

	#region Life Cycle

		private void Start()
		{
			PopulateDictionary();
			_MainCamera = Camera.main;

			// Checking the platform and optimizing the inputs accordingly
			if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
			{
				StartCoroutine(DetectMouseClicks());
			}
			else if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
			{
				StartCoroutine(DetectTouch());
			}
		}

	#endregion

	#region Member Functions

		private void PopulateDictionary()
		{
			_TerrainReactionData = new Dictionary<string, List<ATerrainReaction>>();
			foreach(var terrainReaction in _TerrainReactionList)
			{
				if (!_TerrainReactionData.ContainsKey(terrainReaction.TerrainTag) || _TerrainReactionData[terrainReaction.TerrainTag] == null)
				{
					_TerrainReactionData[terrainReaction.TerrainTag] = new List<ATerrainReaction>();
				}
				_TerrainReactionData[terrainReaction.TerrainTag].Add(terrainReaction.TerrainReaction);
			}
		}

		private IEnumerator DetectMouseClicks()
		{
			while(true)
			{
				if (Input.GetMouseButtonDown(0))
				{
					Vector3 clickPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1);
					CastRay(_MainCamera.ScreenToWorldPoint(clickPosition));
				}

				yield return null;
			}
		}

		private IEnumerator DetectTouch()
		{
			while(true)
			{
				if (Input.touchCount > 0)
				{
					Vector3 touchPosition = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 1);
					CastRay(_MainCamera.ScreenToWorldPoint(touchPosition));
				}
				yield return null;
			}
		}

		private void CastRay(Vector3 positionToCastTo)
		{
			RaycastHit raycastHitInfo;
			Physics.Raycast(_MainCamera.transform.position, (positionToCastTo - _MainCamera.transform.position), out raycastHitInfo, 100.0f, _RaycastLayerMask);
			if (raycastHitInfo.collider != null)
			{
				if (_TerrainReactionData.ContainsKey(raycastHitInfo.collider.tag))
				{
					ExecuteTerrainReaction(raycastHitInfo.collider.tag, raycastHitInfo.collider.gameObject, raycastHitInfo.point);
					Debug.LogFormat("Point hit: {0}", raycastHitInfo.point);
				}
			}
		}

		private void ExecuteTerrainReaction(string terrainTag, GameObject terrain, Vector3 positionOfTouch)
		{
			foreach(var terrainReaction in _TerrainReactionData[terrainTag])
			{
				terrainReaction.React(positionOfTouch, terrain.transform.up);
			}
		}

	#endregion
}

[System.Serializable]
public struct TerrainReactionData
{
	public string TerrainTag;
	public ATerrainReaction TerrainReaction;
}
