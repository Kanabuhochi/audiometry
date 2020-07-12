using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class calibrationValue2 : MonoBehaviour {


	public float noise = 100.0f;
    public bool isToggled = false;

	public Slider slider;
	public Image sft;
	public GameObject toggleSwitch;

	// Use this for initialization
	void Start () {
	toggleSwitch =  GameObject.Find("ToggleSwitch");
	int objects = FindObjectsOfType<calibrationValue2>().Length;
     if (objects != 1)
     {
         Destroy(this.gameObject);
     }
     // if more then one music player is in the scene
     //destroy ourselves
     else
     {
         DontDestroyOnLoad(gameObject);
     }
	}

	 //OnEnable comes first
     private void OnEnable()
     {
         SceneManager.sceneLoaded += SceneManager_sceneLoaded;
     }

	  private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
     {
         if(scene.isLoaded)
         {
             toggleSwitch =  GameObject.Find("ToggleSwitch");
         }
     }
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setCalib () {
		noise = slider.GetComponent<setSlider>().number;
		isToggled = toggleSwitch.GetComponent<toggleScript>().isOn;
	}

}
