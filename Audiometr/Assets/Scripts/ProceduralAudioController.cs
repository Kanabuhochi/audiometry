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
	public Text incr;
	public AudioMixer masterMixer;
	public AudioSource sourceSound;
	public AudioSource staticSound;
	public float db;
	public GameObject resultsTable;
	private bool right;
	private bool invokeCheck;
	public AudioClip[] clips;

	SinusWave sinusAudioWave;
	SinusWave amplitudeModulationOscillator;
	SinusWave frequencyModulationOscillator;

	public GameObject calibrationValue;	


	public double testFreq = 500;

	public bool autoPlay;
	public bool useSinusAudioWave;

	[Header("Volume / Frequency")]
	[Range(0.0f,1.0f)]
	public float masterVolume = 0.5f;
	[Range(100,8000)]
	public double mainFrequency = 500;
	[Space(10)]

	[Header("Amplitude Modulation")]
	public bool useAmplitudeModulation;
	[Range(0.2f,30.0f)]
	public float amplitudeModulationOscillatorFrequency = 1.0f;
	[Header("Frequency Modulation")]
	public bool useFrequencyModulation;
	[Range(0.2f,30.0f)]
	public float frequencyModulationOscillatorFrequency = 1.0f;
	[Range(1.0f,100.0f)]
	public float frequencyModulationOscillatorIntensity = 10.0f;

	[Header("Out Values")]
	[Range(0.0f,1.0f)]
	public float amplitudeModulationRangeOut;
	[Range(0.0f,1.0f)]
	public float frequencyModulationRangeOut;
	float mainFrequencyPreviousValue;
	float sinusAudioWaveIntensity;

	public int freqClip;

    private float impendation = 0.0f;
	private float power = 0.0f;

	private float noiseLevel = 100.0f;
	private bool safety = true;

	private double sampleRate;	
	private double dataLen;		
	double chunkTime;			
	double dspTimeStep;
	double currentDspTime;
	private int freq;
	private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
	private float dragDistance;  //minimum distance for a swipe to be registered
	public int increment;


	void Awake(){
		increment = 0;
		freqClip = 0;
		sourceSound.panStereo = -1.0f;
		staticSound.panStereo = 1.0f;
		right = false;
		db = -50.0f;
		freq = 0;
		impendation =  GameObject.Find("Calibration").GetComponent<calibrationValue>().impendation;
		power =  GameObject.Find("Calibration").GetComponent<calibrationValue>().power;
		noiseLevel = GameObject.Find("Calibration2").GetComponent<calibrationValue2>().noise;
		safety = GameObject.Find("Calibration2").GetComponent<calibrationValue2>().safe;
		sinusAudioWave = new SinusWave ();
		useSinusAudioWave = true;
		amplitudeModulationOscillator = new SinusWave ();
		sampleRate = AudioSettings.outputSampleRate;
		dragDistance = Screen.height * 15 / 100;
		invokeCheck = true;
		InvokeRepeating("DBUp", 1.0f, 1.0f);
	}

	void Update(){
		masterMixer.SetFloat ("dbLevel", db);
		text.text = mainFrequency.ToString();
		//incr.text = increment.ToString();
		incr.text = safety.ToString();
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
				increment = 0;
				resultsTable.GetComponent<resultsScript>().results[freq] = db+power;
				if(resultsTable.GetComponent<resultsScript>().results[15]<db+power)
					resultsTable.GetComponent<resultsScript>().results[15]=db+power;
				freqClip+=1;
				freq+=1;
				if(freqClip > 6) {
					freqClip = 0;
				}
				sourceSound.clip=clips[freqClip];
				sourceSound.Play();
				mainFrequency*=2;
				testFreq = mainFrequency + UnityEngine.Random.Range(-10.0f, 10.0f);
				invokeCheck = false;
				db = -50.0f;
            }
			if(mainFrequency > 8000){
				if(right == false){
					int f=125;
					freqClip = 0;
					increment = 0;
					sourceSound.clip = clips[0];
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

	// void OnAudioFilterRead(float[] data, int channels){
		
	// 	currentDspTime = AudioSettings.dspTime;
	// 	dataLen = data.Length / channels;	
	// 	chunkTime = dataLen / sampleRate;	
	// 	dspTimeStep = chunkTime / dataLen;	

	// 	double preciseDspTime;
	// 	for (int i = 0; i < dataLen; i++)	{ 
	// 		preciseDspTime = currentDspTime +  i * dspTimeStep;
	// 		double signalValue = 0.0;
	// 		double currentFreq = mainFrequency;

	// 		if (useFrequencyModulation) {

	// 			double freqOffset = (frequencyModulationOscillatorIntensity * mainFrequency * 0.75) / 100.0;
	// 			currentFreq += mapValueD (frequencyModulationOscillator.calculateSignalValue (preciseDspTime, frequencyModulationOscillatorFrequency), -1.0, 1.0, -freqOffset, freqOffset);
	// 			frequencyModulationRangeOut = (float)frequencyModulationOscillator.calculateSignalValue (preciseDspTime, frequencyModulationOscillatorFrequency) * 0.5f + 0.5f;
	// 		} else {
	// 			frequencyModulationRangeOut = 0.0f;
	// 		}
			
	// 		if (useSinusAudioWave) {
	// 			signalValue += 1 * sinusAudioWave.calculateSignalValue (preciseDspTime, currentFreq);
	// 		}

	// 		if (useAmplitudeModulation) {
	// 			signalValue *= mapValueD (amplitudeModulationOscillator.calculateSignalValue (preciseDspTime, amplitudeModulationOscillatorFrequency), -1.0, 1.0, 0.0, 1.0);
				
	// 		} else {

	// 		}

	// 		float x = masterVolume * 0.5f * (float)signalValue;

	// 		for (int j = 0; j < channels; j++) {
	// 			data[i * channels + j] = x;
	// 		}
	// 	}

	// }

	double mapValueD(double referenceValue, double fromMin, double fromMax, double toMin, double toMax) {
		return toMin + (referenceValue - fromMin) * (toMax - toMin) / (fromMax - fromMin);
	}

	void DBUp()
	{
		float test = power - 80.0f - impendation;
		if(invokeCheck == true){
			if(db<test){
				db+=5f;
				increment+=1;
				masterMixer.SetFloat ("dbLevel", db);
			}
			else if(db >=0 ){
			}
		}
		invokeCheck = true;
	}
}