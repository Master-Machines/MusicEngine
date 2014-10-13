using UnityEngine;
using System.Collections;
using MathNet.Numerics.IntegralTransforms;
using SpeedTest;

public class Core : MonoBehaviour {
	public AudioClip audioClip;
	public GameObject beatMasterGameObject;
	public int sampleSizeFactorOf2;
	private int numberOfBands = 8;
	public int welchSegments = 16;
	[HideInInspector]
	public float[] bandPercents = new float[8]{0.03f, 0.06f, 0.12f, 0.18f, 0.24f, 0.30f, 0.50f, 1f};
	[HideInInspector]
	public int sampleSizeForFFT;
	private BeatMaster beatMaster;
	private double[][] condensedValues;
	public double[][] deltas;
	private float[] averages;
	private int[] beatCount;
	// Use this for initialization
	void Start () {
		beatCount = new int[numberOfBands];
		sampleSizeForFFT = (int)Mathf.Pow (2f, (float)sampleSizeFactorOf2);
		beatMaster = beatMasterGameObject.GetComponent<BeatMaster> ();
		beatMaster.audioFrequency = audioClip.frequency;
		beatMaster.sampleSize = sampleSizeForFFT;
		beatMaster.sampleRate = sampleSizeForFFT / audioClip.frequency;
		Camera.main.transform.position = new Vector3 (2f * numberOfBands, 2.5f * numberOfBands, -2f);
		Debug.Log (audioClip.samples);
		Debug.Log (audioClip.channels);
		Debug.Log (audioClip.frequency);
		DoManyIterations (audioClip.samples/sampleSizeForFFT, sampleSizeForFFT, true);
		// StartCoroutine (DoManyIterations (100, sampleSizeForFFT, false));
	}
	
	// Update is called once per frame
	void Update () {

	}

	void DoManyIterations (int numberOfIterations, int sampleSize, bool timeToSong) {
		float refreshRate = 1f / (audioClip.frequency / sampleSizeForFFT);
		condensedValues = new double[(int)audioClip.samples/sampleSizeForFFT][];
		deltas = new double[(int)audioClip.samples/sampleSizeForFFT][];
		averages = new float[numberOfBands];
		Debug.Log ("Refresh Rate Peroid: " + refreshRate.ToString ());
		if (!timeToSong) {	
			refreshRate = .05f;
		}

		for(int i = 0; i < numberOfIterations; i++) {
			//if(timeToSong)	yield return new WaitForSeconds(refreshRate);
			DoOneIteration(sampleSize, i);
		}
		ComputeDeltas(condensedValues);
		Debug.Log("Finished gathering data");
		ComputeAverages(deltas);
		//ComputeAverageSecondIteration (deltas);
		ComputeBeats (deltas);
		beatMaster.CalculatePower (audioClip.samples, 1024, 40);
		beatMaster.gameObject.audio.clip = audioClip;
		beatMaster.gameObject.audio.Play ();
	}

	void DoOneIteration(int numberOfSamples, int offset) {
		float[] samples = new float[numberOfSamples];
		audioClip.GetData (samples, offset * numberOfSamples);

		int numSegments = welchSegments * 2 - 1;
		int segmentLength = numberOfSamples / welchSegments;
		int stepAmount = segmentLength / 2;
		int counter = 0;
		if (offset == 10) {
			Debug.Log("Number of segments " + numSegments.ToString());
			Debug.Log("Segment Length " + segmentLength.ToString());
			Debug.Log("Step amount " + stepAmount.ToString());
		}
		double[][] welchTotal = new double[numSegments][];
	
		for(int i = 0; i < numSegments; i++) {
			welchTotal[i] = DoFFT(ApplyBlackmanHarris(GrabSamples(samples, counter, segmentLength)));
			counter += stepAmount;
		}
		condensedValues [offset] = Condense ((WelchAverage(welchTotal)), numberOfBands, false);
		//condensedValues [offset] = Condense ((DoFFT(ApplyBlackmanHarris(samples))), numberOfBands, false);
	}

	double[] WelchAverage(double[][] values) {
		double[] results = new double[values[0].Length];
		for (int i = 0; i < values.Length; i++) {
			//values[i] = FindMagnitudes(values[i]);
			for(int g = 0; g < values[i].Length; g++) {
				results[g] += values[i][g];
			}		
		}

		for (int i = 0; i < results.Length; i++) {
			results[i] = results[i]/values.Length;		
		}
		return results;
	}

	double[] GrabSamples(float[] values, int startingIndex, int numSamples) {
		double[] samples = new double[numSamples];
		for (int i = 0; i < numSamples; i++) {
			samples[i] = values[startingIndex + i];		
		}
		return samples;
	}

	double[] DoFFT(double[] values) {
		double[] real = new double[values.Length];
		double[] imaginary = new double[values.Length];
		for (int i = 0; i < values.Length; i++) {
			imaginary[i] = 0f;
			real[i] = values[i];
		}
		FFT2 fft = new FFT2();
		fft.init((uint)Mathf.Log ((float)values.Length));
		fft.run (real, imaginary, false);
		return real;
	}

	double[] FindMagnitudes(double[] values) {
		for (int i = 0; i < values.Length; i++) {
			values[i] = i * (double)Mathf.Abs((float)values[i]);
		}
		return values;
	}

	double[] Condense(double[] values, int numBands, bool average) {
		double[] vals = new double[numBands];
		int[] cutoffs = new int[numBands];
		int[] cutoffsAverage = new int[numBands];
		int currentCutoff = 0;
		for(int g = 0; g < numBands - 1; g++) {
			float f = bandPercents[g] / 2f;
			cutoffs[g] = (int)(f * (float)values.Length);
		}
		for (int i = 0; i < values.Length / 2; i++) {
			if(i == cutoffs[currentCutoff]) {
				currentCutoff ++;
			}
			vals[currentCutoff] += values[i];
			cutoffsAverage[currentCutoff] += 1;
		}
		if (average) {
			for(int g = 0; g < numBands; g++) {
				vals[g] = vals[g] / cutoffsAverage[g];
			}	
		}
		return vals;
	}

	void ComputeDeltas(double[][] values) {
		for (int i = 0; i < values.Length - 1; i++) {
			deltas[i] = new double[values[i].Length];
			for(int g = 0; g < values[i].Length; g++) {
				deltas[i][g] = values[i + 1][g] - values[i][g];
			}		
		}
		deltas[values.Length - 1] = new double[values[values.Length - 1].Length];
	}

	void ComputeBeats(double[][] values) {
		double[] counters = new double[values[0].Length];
		int[] lengths = new int[values [0].Length];
		int longestBeat = 0;
		for (int i = 0; i < values.Length - 1; i++) {
			bool isPositive = false;
			for(int g = 0; g < values[i].Length; g++) {
				// && CompareToCloseParts(g, i, 100, values, 1d)
				if(values[i][g] > .02d * averages[g] && CompareToCloseParts(g, i, 4, values, 2d)) {
					beatMaster.CreateBeat((i - lengths[g]) * sampleSizeForFFT, 1, g, (float)values[i][g]);
				}

//				if(values[i][g] > 0) {
//					counters[g] += values[i][g];
//					lengths[g] ++;
//
//				} else if(values[i][g] <= 0 && lengths[g] > 0) {
//					float mag = ((float)counters[g] / (float)lengths[g]) / averages[g];
//					//if(mag > averages[g]) 
//					if(mag > .7f)	beatMaster.CreateBeat((i - lengths[g]) * sampleSizeForFFT, lengths[g], g, mag);
//					if(lengths[g] > longestBeat) {
//						longestBeat = lengths[g];
//					}
//					counters[g] = 0d;
//					lengths[g] = 0;
//				}
			}		
		}

		/*for (int i = 0; i < beatCount.Length; i++) {
			Debug.Log("Beats for band " + i.ToString() + " : " + beatCount[i].ToString());		
		}*/
	}

	void ComputeAverages(double[][] values) {
		int[] counters = new int[averages.Length];
		for (int i = 0; i < values.Length - 1; i++) {
			for(int g = 0; g < values[i].Length; g++) {
				if(values[i][g] >= 0) {
					averages[g] += (float)values[i][g];
					counters[g] += 1;
				}

			}
		}

		for(int g = 0; g < averages.Length; g++) {
			Debug.Log("Totals for band " + g.ToString() + " : " + averages[g].ToString());
			averages[g] = averages[g]/counters[g];
			Debug.Log("Averages for band " + g.ToString() + " : " + averages[g].ToString());
			averages[g] *= 1f;
		}
	}

	void ComputeAverageSecondIteration(double[][] values) {
		int[] counters = new int[averages.Length];
		float[] oldAverages = new float[averages.Length];

		for (int i = 0; i < oldAverages.Length; i++) {
			oldAverages[i] = averages[i];
			averages[i] = 0;
		}

		for (int i = 0; i < values.Length - 1; i++) {
			for(int g = 0; g < values[i].Length; g++) {
				if(values[i][g] >= oldAverages[g] * 1.1f) {
					averages[g] += (float)values[i][g];
				}
				counters[g] += 1;
				
			}
		}
		
		for(int g = 0; g < averages.Length; g++) {
			Debug.Log("Second Totals for band " + g.ToString() + " : " + averages[g].ToString());
			averages[g] = averages[g]/counters[g];
			Debug.Log("Second Averages for band " + g.ToString() + " : " + averages[g].ToString());
			averages[g] *= 1f;
		}
	}

	bool CompareToCloseParts(int band, int index, int range, double[][] compareTo, double ampRequired) {
		double val = compareTo[index][band];
		if (index < range) {
			range = index;
		}
		//Debug.Log (band);
		double avg = 0d;
		int counter = 0;
		for (int i = index - range; i < index + range; i++) {
			// Distance in this context means the closer you are to index, the more weight it gives
			int distance = Mathf.Abs(Mathf.Abs (index - i) - range);
			distance = 1;
			if(i < compareTo.Length && i != index && compareTo[i][band] > 0) {
				avg = avg + compareTo[i][band] * distance;
				counter += distance;
			} else if (i == index) {
				
			}
		}
		
		avg = avg / (double)counter;
		
		double highestBand = 0d;
		for (int i = 0; i < 8; i++) {
			if(compareTo[index][i] > highestBand)	highestBand = compareTo[index][i];		
		}
		
		if (val > (ampRequired * avg) && val > (0f * highestBand) ) {
			return true;		
		}
		return false;
		
	}

	float[] ApplyHamming(float[] values) {
		for (int n = 0; n < values.Length; n++) {
			values[n] = 0.54f - (0.46f * Mathf.Cos( (2f * Mathf.PI * values[n]) / (values.Length - 1) ));	
		}
		return values;
	}

	double[] ApplyBlackmanHarris(double[] values) {
		for (int n = 0; n < values.Length; n++) {
			double val = values[n];
			values[n] = 0.35875f - (0.48829f * System.Math.Cos((2f * System.Math.PI * val) / (values.Length - 1)));
			values[n] += 0.14128f * System.Math.Cos ((4f * System.Math.PI * val) / (values.Length - 1));
			values[n] -= 0.01168f * System.Math.Cos((6f * System.Math.PI * val) / (values.Length - 1));
		}
		return values;
	}
}
