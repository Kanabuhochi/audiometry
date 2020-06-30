using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void startInstruct()
	{
		SceneManager.LoadScene("Instruct"); 
	}

	public void startInstruct2()
	{
		SceneManager.LoadScene("Instruct2"); 
	}

	public void startMeasure()
	{
		SceneManager.LoadScene("Measure"); 
	}

	public void About()
	{
		SceneManager.LoadScene("About"); 
	}

	public void Calibrate()
	{
		SceneManager.LoadScene("Calibration"); 
	}

	public void CalibInfo()
	{
		SceneManager.LoadScene("NewCalib"); 
	}

	public void Back()
	{
		SceneManager.LoadScene("Menu"); 
	}

	public void CalibConfig()
	{
		SceneManager.LoadScene("NewCalib2"); 
	}


	public void Exit()
	{
		Application.Quit();
	}
	
}
