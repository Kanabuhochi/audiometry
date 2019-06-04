using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

[RequireComponent(typeof (AudioSource))]
public class Calibration : MonoBehaviour {

	public AudioMixer masterMixer;
	public AudioSource sourceSound;

	private float calibration;
	private int count;
	private AudioSource microphone;

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
	public double mainFrequency = 125;
	[Space(10)]

	[Header("Tone Adjustment")]
	public bool useSinusAudioWave;
	[Range(0.0f,1.0f)]
	public float sinusAudioWaveIntensity = 0.25f;

	[Space(10)]

	[Header("Amplitude Modulation")]
	public bool useAmplitudeModulation = true;
	[Range(0.2f,30.0f)]
	public float amplitudeModulationOscillatorFrequency = 1.0f;

	[Header("Out Values")]
	[Range(0.0f,1.0f)]
	public float amplitudeModulationRangeOut;
	[Range(0.0f,1.0f)]
	public float frequencyModulationRangeOut;

	float mainFrequencyPreviousValue;

	private double sampleRate;	
	private double dataLen;		
	double chunkTime;			
	double dspTimeStep;
	double currentDspTime;
	private int freq;



	void Awake(){
		count = 0;
		//var audio = GetComponent<AudioSource>();
	//	audio.clip = Microphone.Start("Start",true,10,44100);
 
		mainFrequency = 125;
		useAmplitudeModulation = true;
		sourceSound.panStereo = -1.0f;
		right = false;
		db = -80.0f;
		freq = 0;
		sinusAudioWave = new SinusWave ();
		useSinusAudioWave = true;
		amplitudeModulationOscillator = new SinusWave ();
		sampleRate = AudioSettings.outputSampleRate;
		invokeCheck = true;
		InvokeRepeating("DBUp", 1f, 1f);

		
	}

	void Update(){
		masterMixer.SetFloat ("dbLevel", db);
		if(mainFrequency > 8000){
			if(right == false){
				int f=125;
				for(int i = 0; i<7;i++){

					f=f*2;
				}
				mainFrequency = 125;
				db = -80.0f;
				sourceSound.panStereo = 1.0f;
				right = true;
			}
			else if(right == true){
				int f=125;
				for(int i = 7; i<14;i++){

					f=f*2;
				}
				db = -80.0f;
			}
		}
	//	float[] spectrum = new float[256];
		//calibration = AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);
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
				amplitudeModulationRangeOut = (float)amplitudeModulationOscillator.calculateSignalValue (preciseDspTime, amplitudeModulationOscillatorFrequency) * 0.5f + 0.5f;
			} else {
				amplitudeModulationRangeOut = 0.0f;
			}
			float x = masterVolume * 0.5f * (float)signalValue;
			for (int j = 0; j < channels; j++) {
				data[i * channels + j] = x;
			}
			float[] spectrum = new float[256];
//			calibration = AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);
		}
	}
	double mapValueD(double referenceValue, double fromMin, double fromMax, double toMin, double toMax) {
		return toMin + (referenceValue - fromMin) * (toMax - toMin) / (fromMax - fromMin);
	}

	void DBUp()
	{
		while(calibration != mainFrequency){
			if(invokeCheck == true){
				if(db<25){
					db+=5f;
					masterMixer.SetFloat ("dbLevel", db);
				}
				else if(db >=25 ){
					mainFrequency*=2;
					db= -80.0f;
					count+=1;
				}
			}
			invokeCheck = true;
			if(count>=13){
				SceneManager.LoadScene("CalibInfo2"); 
			}
			
	//	}
	}

		 
	}
}