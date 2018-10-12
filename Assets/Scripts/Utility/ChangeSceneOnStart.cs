using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneOnStart : MonoBehaviour 
{
	[SerializeField]
	private string _SceneToChangeToOnStart = "MainMenu 1";

	// Use this for initialization
	void Start () 
	{
		SceneManager.LoadScene(_SceneToChangeToOnStart);
	}

}
