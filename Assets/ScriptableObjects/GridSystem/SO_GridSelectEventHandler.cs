using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "GridSelectEventHandler", menuName="FoodIsLove/GridSystem/GridSelectEventHandler")]
public class SO_GridSelectEventHandler : ScriptableObject 
{
	private event GridSelectAction _GridSelectEvent;

	public void Initialize()
	{
		_GridSelectEvent = null;
	}

	public void SubscribeToGridSelectEvent(GridSelectAction action)
	{
		_GridSelectEvent += action;
	}

	public void InvokeEvent(GridPosition selectedPosition, GridProp selectedObject)
	{
		if (_GridSelectEvent != null)
		{
			_GridSelectEvent.Invoke(selectedPosition, selectedObject);
		}
	}
}

public delegate void GridSelectAction(GridPosition gridPosition, GridProp selectedObject);