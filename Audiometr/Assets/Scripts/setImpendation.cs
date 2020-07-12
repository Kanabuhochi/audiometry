using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class setImpendation : MonoBehaviour {

	public GameObject setter;
	public InputField imp;


	// Use this for initialization
	void Start () {
		setter = GameObject.Find("Calibration");
		imp.GetComponent<InputField>().text = setter.GetComponent<calibrationValue>().impendation;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
