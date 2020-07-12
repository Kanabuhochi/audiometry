using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class calibrationValue : MonoBehaviour {

	public string impendation;
    public string power;
	public bool isToggled = false;

	public InputField imp;
	public InputField pow;
	public GameObject toggleSwitch;



	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this.gameObject);
		toggleSwitch =  GameObject.Find("Toggle Switch");

		if(impendation != null)
		{
			imp.GetComponent<InputField>().text = impendation;
		}

		if(power != null) {
			pow.GetComponent<InputField>().textComponent.text = power;
		}

		

		

	int objects = FindObjectsOfType<calibrationValue>().Length;
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

	private void OnEnable()
     {
         SceneManager.sceneLoaded += SceneManager_sceneLoaded;
     }

	  private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
     {
         if(scene.isLoaded)
         {
             toggleSwitch =  GameObject.Find("Toggle Switch");
         }
     }
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setCalib () {
		impendation =  imp.GetComponent<InputField>().text;
		power =  pow.GetComponent<InputField>().text;
		isToggled = toggleSwitch.GetComponent<toggleScript>().isOn;
	}
}
