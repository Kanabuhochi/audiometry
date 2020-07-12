using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class setPower : MonoBehaviour {

	public GameObject setter;
	public InputField power;


	// Use this for initialization
	void Start () {
		setter = GameObject.Find("Calibration");
		power.GetComponent<InputField>().text = setter.GetComponent<calibrationValue>().power;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
