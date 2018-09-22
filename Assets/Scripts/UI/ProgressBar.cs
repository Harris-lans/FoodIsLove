using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour 
{
	public SO_Score mScore;
	private Slider ProgressBarSlider;
	private void Start() 
	{
		ProgressBarSlider = GetComponent<Slider>();
		ProgressBarSlider.value = 0f;	
	}

	private void Update() 
	{
		ProgressBarSlider.value = mScore.Score; //Update Progress Bar fill amount
	}

}
