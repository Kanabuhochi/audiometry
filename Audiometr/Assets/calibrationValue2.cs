using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class calibrationValue2 : MonoBehaviour {


	public float noise;
    public bool safe;

	public InputField ns;
	public Image sft;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setCalib () {
		noise=float.Parse(ns.text);
		safe = GameObject.Find("ToggleSwitch").GetComponent<toggleScript>().isOn;
	}

}
