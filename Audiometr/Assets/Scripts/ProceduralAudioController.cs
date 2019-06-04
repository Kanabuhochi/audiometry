using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

[RequireComponent(typeof (AudioSource))]
public class ProceduralAudioController : MonoBehaviour {

	public Text text;
	public AudioMixer masterMixer;
	public AudioSource sourceSound;
	public AudioSource staticSound;
	public float db;
	public GameObject resultsTable;
	private bool right;
	private bool invokeCheck;

	SinusWave sinusAudioWave;
	SinusWave amplitudeModulationOscillator;

	public bool autoPlay;

	[Header("Volume / Frequency")]
	[Range(0.0f,1.0f)]
	public float masterVolume = 0.5f;
	[Range(100,8000)]
	public double mainFrequency = 500;
	[Space(10)]

	[Header("Tone Adjustment")]
	public bool useSinusAudioWave;
	[Range(0.0f,1.0f)]
	public float sinusAudioWaveIntensity = 0.25f;

	[Space(10)]

	[Header("Amplitude Modulation")]
	public bool useAmplitudeModulation;
	[Range(0.2f,30.0f)]
	public float amplitudeModulationOscillatorFrequency = 1.0f;

	float mainFrequencyPreviousValue;

	private double sampleRate;	
	private double dataLen;		
	double chunkTime;			
	double dspTimeStep;
	double currentDspTime;
	private int freq;
	private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
	private float dragDistance;  //minimum distance for a swipe to be registered


	void Awake(){
		sourceSound.panStereo = -1.0f;
		staticSound.panStereo = 1.0f;
		right = false;
		db = -80.0f;
		freq = 0;
		sinusAudioWave = new SinusWave ();
		useSinusAudioWave = true;
		amplitudeModulationOscillator = new SinusWave ();
		sampleRate = AudioSettings.outputSampleRate;
		dragDistance = Screen.height * 15 / 100;
		invokeCheck = true;
		InvokeRepeating("DBUp", 3.0f, 3.0f);
	}

	void Update(){
		masterMixer.SetFloat ("dbLevel", db);
		text.text = mainFrequency.ToString();
		if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0); 
            if (touch.phase == TouchPhase.Began) 
            {
                fp = touch.position;
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved) {
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended) 
            {
                lp = touch.position;  
                if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                {
                 
                    if (lp.y > fp.y)  {
                    }
                    else
                    {   
                    }
                }
                else
                {  
                }
				resultsTable.GetComponent<resultsScript>().results[freq] = db+80.0f;
				if(resultsTable.GetComponent<resultsScript>().results[15]<db+80.0f)
					resultsTable.GetComponent<resultsScript>().results[15]=db+80.0f;

				freq+=1;
				mainFrequency*=2;
				invokeCheck = false;
				db = -80.0f;
            }
			
        
			if(mainFrequency > 8000){
				if(right == false){
					int f=125;
					for(int i = 0; i<7;i++){
						f=f*2;
					}
					mainFrequency = 125;
					db = -80.0f;
					sourceSound.panStereo = 1.0f;
					staticSound.panStereo = -1.0f;
					right = true;
				}
				else if(right == true){
					int f=125;
					for(int i = 7; i<14;i++){
						f=f*2;
					}
					db = -80.0f;
					SceneManager.LoadScene("Results");
				}	
			}
		}
	}

	void OnAudioFilterRead(float[] data, int channels){
		
		currentDspTime = AudioSettings.dspTime;
		dataLen = data.Length / channels;	
		chunkTime = dataLen / sampleRate;	
		dspTimeStep = chunkTime / dataLen;	

		double preciseDspTime;
		for (int i = 0; i < dataLen; i++)	{ 
			preciseDspTime = currentDspTime +  i * dspTimeStep;
			double signalValue = 0.0;
			double currentFreq = mainFrequency;
			
			if (useSinusAudioWave) {
				signalValue += sinusAudioWaveIntensity * sinusAudioWave.calculateSignalValue (preciseDspTime, currentFreq);
			}

			if (useAmplitudeModulation) {
				signalValue *= mapValueD (amplitudeModulationOscillator.calculateSignalValue (preciseDspTime, amplitudeModulationOscillatorFrequency), -1.0, 1.0, 0.0, 1.0);
				
			} else {

			}

			float x = masterVolume * 0.5f * (float)signalValue;

			for (int j = 0; j < channels; j++) {
				data[i * channels + j] = x;
			}
		}

	}

	double mapValueD(double referenceValue, double fromMin, double fromMax, double toMin, double toMax) {
		return toMin + (referenceValue - fromMin) * (toMax - toMin) / (fromMax - fromMin);
	}

	void DBUp()
	{
		if(invokeCheck == true){
			if(db<0){
				db+=5f;
				masterMixer.SetFloat ("dbLevel", db);
			}
			else if(db >=0 ){
			}
		}
		invokeCheck = true;
	}
}
