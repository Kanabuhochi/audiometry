using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System;

[RequireComponent(typeof (AudioSource))]
public class ProceduralAudioController : MonoBehaviour {

	public AudioMixer masterMixer;
	public AudioSource sourceSound;
	public AudioSource staticSound;
	public float db;
	public GameObject resultsTable;
	private bool right;

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
		InvokeRepeating("DBUp", 3.0f, 3.0f);

		
	}

	void Update(){
		masterMixer.SetFloat ("dbLevel", db);

		if (Input.touchCount == 1) // user is touching the screen with a single touch
        {
            Touch touch = Input.GetTouch(0); // get the touch
            if (touch.phase == TouchPhase.Began) //check for the first touch
            {
                fp = touch.position;
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
            {
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
            {
                lp = touch.position;  //last touch position. Ommitted if you use list

                //Check if drag distance is greater than 20% of the screen height
                if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                {//It's a drag
                 //check if the drag is vertical or horizontal
                    if (lp.y > fp.y)  //If the movement was up
                    {   //Up swipe  
                    }
                    else
                    {   //Down swipe  
                    }
                }
                else
                {   //It's a tap as the drag distance is less than 20% of the screen height
                }
				resultsTable.GetComponent<resultsScript>().results[freq] = db+80.0f;
	//			resultsTable.results[freq] = db+80.0f;
				freq+=1;
				mainFrequency*=2;
				db = -80.0f;
            }
        }
		if(mainFrequency > 8000){
			if(right == false){
				int f=125;
				for(int i = 0; i<7;i++){
	//				Debug.Log("FrequencyL: " + f + "DecibelsL:" + resultsTable.results[i]);
					Debug.Log("FrequencyL: " + f + "DecibelsL:" + resultsTable.GetComponent<resultsScript>().results[i]);
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
	//				Debug.Log("FrequencyR: " + f + "DecibelsR:" + resultsTable.results[i]);
					Debug.Log("FrequencyR: " + f + "DecibelsR:" + resultsTable.GetComponent<resultsScript>().results[i]);
					f=f*2;
				}
				db = -80.0f;
				SceneManager.LoadScene("Results");
			}

		}


		//if (autoPlay) {
		//	if(!useSinusAudioWave){
		//		useSinusAudioWave = true;
		//	}
			
			//mainFrequency = Mathf.PingPong (Time.time *200.0f, 1900.0f) + 100.0f;
			//sinusAudioWaveIntensity = Mathf.PingPong (Time.time * 0.5f, 1.0f);
	//	}



	}

	void OnAudioFilterRead(float[] data, int channels){
		/* This is called by the system
		suppose: sampleRate = 48000
		suppose: data.Length = 2048
		suppose: channels = 2
		then:
		dataLen = 2048/2 = 1024
		chunkTime = 1024 / 48000 = 0.0213333... so the chunk time is around 21.3 milliseconds.
		dspTimeStep = 0.0213333 / 1024 = 2.083333.. * 10^(-5) = 0.00002083333..sec = 0.02083 milliseconds
			keep note that 1 / dspTimeStep = 48000 ok!		
		*/

		currentDspTime = AudioSettings.dspTime;
		dataLen = data.Length / channels;	// the actual data length for each channel
		chunkTime = dataLen / sampleRate;	// the time that each chunk of data lasts
		dspTimeStep = chunkTime / dataLen;	// the time of each dsp step. (the time that each individual audio sample (actually a float value) lasts)

		double preciseDspTime;
		for (int i = 0; i < dataLen; i++)	{ // go through data chunk
			preciseDspTime = currentDspTime +  i * dspTimeStep;
			double signalValue = 0.0;
			double currentFreq = mainFrequency;
			
			if (useSinusAudioWave) {
				signalValue += sinusAudioWaveIntensity * sinusAudioWave.calculateSignalValue (preciseDspTime, currentFreq);
			}

			if (useAmplitudeModulation) {
				signalValue *= mapValueD (amplitudeModulationOscillator.calculateSignalValue (preciseDspTime, amplitudeModulationOscillatorFrequency), -1.0, 1.0, 0.0, 1.0);
//				amplitudeModulationRangeOut = (float)amplitudeModulationOscillator.calculateSignalValue (preciseDspTime, amplitudeModulationOscillatorFrequency) * 0.5f + 0.5f;
			} else {
//				amplitudeModulationRangeOut = 0.0f;
			}

			float x = masterVolume * 0.5f * (float)signalValue;

			for (int j = 0; j < channels; j++) {
				data[i * channels + j] = x;
			}
		}

	}

	double mapValueD(double referenceValue, double fromMin, double fromMax, double toMin, double toMax) {
		/* This function maps (converts) a Double value from one range to another */
		return toMin + (referenceValue - fromMin) * (toMax - toMin) / (fromMax - fromMin);
	}

	void DBUp()
	{
		if(db<0){
			db+=5.0f;
			masterMixer.SetFloat ("dbLevel", db);
		}
		else if(db >=0 ){
		}
	}
}
