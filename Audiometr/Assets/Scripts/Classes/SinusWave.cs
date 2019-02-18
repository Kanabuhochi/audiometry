using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SinusWave{
	
	double currentSignalTime;			
	double currentSignalFrequency;		
	double currentSignalPhaseOffset;	

	double currentSignaOutlValue;		

	public SinusWave(){
		currentSignalFrequency = 400.0;
		currentSignalPhaseOffset = 0.0;
	}

	public double calculateSignalValue(double newSignalTime, double newSignalFrequency){

		if (currentSignalFrequency != newSignalFrequency) {

			double currentSignalPeriod = 1.0 / currentSignalFrequency;
			
			double currentNumberOfSignalCycles = (currentSignalTime / currentSignalPeriod) + (currentSignalPhaseOffset / (2.0 * Math.PI));
			double currentSignalCyclePosition = currentNumberOfSignalCycles % 1.0;	

			double newSignalPeriod = 1.0 / newSignalFrequency;
			double newNumberOfSignalCycles = currentSignalTime / newSignalPeriod;
			double newSignalCyclePosition = newNumberOfSignalCycles % 1.0;		

			double cycleDifference = currentSignalCyclePosition - newSignalCyclePosition;
			double phaseDifference = cycleDifference * Math.PI * 2.0;

			currentSignalPhaseOffset = phaseDifference;

			currentSignalFrequency = newSignalFrequency;
			currentSignalTime = newSignalTime;

			currentSignaOutlValue = Math.Sin (currentSignalTime * 2.0 * Math.PI * currentSignalFrequency + currentSignalPhaseOffset);
			return currentSignaOutlValue;
		} else {
			currentSignalFrequency = newSignalFrequency;
			currentSignalTime = newSignalTime;
			currentSignaOutlValue = Math.Sin (currentSignalTime * 2.0 * Math.PI * currentSignalFrequency + currentSignalPhaseOffset);
			return currentSignaOutlValue;
		}
	}

}