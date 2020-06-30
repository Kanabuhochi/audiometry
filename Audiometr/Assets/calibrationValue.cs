using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class calibrationValue : MonoBehaviour {

	public float impendation;
    public float power;

	public InputField imp;
	public InputField pow;


	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setCalib () {
			impendation=float.Parse(imp.text);
			power=float.Parse(pow.text);
		
	}

}
