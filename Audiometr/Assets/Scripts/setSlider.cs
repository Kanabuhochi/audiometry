using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class setSlider : MonoBehaviour {

	public GameObject set;
	public Slider ui;
	public float number;

	// Use this for initialization
	void Start () {
		set = GameObject.Find("Calibration2");
		ui.value = set.GetComponent<calibrationValue2>().noise;
		number = ui.value;
	}
	
	// Update is called once per frame
	void Update () {
		number = ui.value;
	}
}
