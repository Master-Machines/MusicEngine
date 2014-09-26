using UnityEngine;
using System.Collections;
using MathNet.Numerics.IntegralTransforms;
using SpeedTest;

public class Core : MonoBehaviour {
	public AudioSource audioSource;
	public AudioClip audioClip;
	public GameObject visualMasterObject;
	public int sampleSizeFactorOf2;
	public int numberOfBands;
	public int welchSegments = 16;
	private int sampleSizeForFFT;
	private VisualMaster visualMaster;
	private double[][] condensedValues;
	private double[][] deltas;
	private float[] averages;
	private int[] beatCount;
	// Use this for initialization
	void Start () {
		beatCount = new int[numberOfBands];
		sampleSizeForFFT = (int)Mathf.Pow (2f, (float)sampleSizeFactorOf2);
		visualMaster = visualMasterObject.GetComponent<VisualMaster> ();
		Camera.main.transform.position = new Vector3 (2.5f * numberOfBands, 3f * numberOfBands, 0f);
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
		ComputeBeats (deltas);
		visualMasterObject.audio.clip = audioClip;
		visualMasterObject.audio.Play ();
	}

	void DoOneIteration(int numberOfSamples, int offset) {
		float[] samples = new float[numberOfSamples];
		audioClip.GetData (samples, offset * numberOfSamples);

		int numSegments = welchSegments * 2 - 1;
		int segmentLength = numberOfSamples / welchSegments;
		int stepAmount = segmentLength / 2;
		int counter = 0;
		double[][] welchTotal = new double[numSegments][];
	
		for(int i = 0; i < numSegments; i++) {
			welchTotal[i] = DoFFT(ApplyHamming(GrabSamples(samples, counter, segmentLength)));
			counter += stepAmount;
		}
		condensedValues [offset] = Condense (FindMagnitudes(WelchAverage(welchTotal)), numberOfBands, true);
		//visualMaster.CreateRow (real);
	}

	double[] WelchAverage(double[][] values) {
		double[] results = new double[values.Length];
		for (int i = 0; i < values.Length; i++) {
			for(int g = 0; g < values[i].Length; g++) {
				results[i] += values[i][g];
			}		
		}

		for (int i = 0; i < results.Length; i++) {
			results[i] = results[i]/values.Length;		
		}
		return results;
	}

	float[] GrabSamples(float[] values, int startingIndex, int numSamples) {
		float[] samples = new float[numSamples];
		for (int i = 0; i < numSamples; i++) {
			samples[i] = values[startingIndex + i];		
		}
		return samples;
	}

	double[] DoFFT(float[] values) {
		double[] real = new double[values.Length];
		double[] imaginary = new double[values.Length];
		for (int i = 0; i < values.Length; i++) {
			imaginary[i] = 0f;
			real[i] = (double)values[i];
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
		for(int g = 0; g < numBands; g++) {
			float f = (float)(g + 1) / (float)numBands;
			cutoffs[g] = (int)(f * (float)values.Length / 2f);
		}
		for (int i = 0; i < values.Length/2; i++) {
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
	}

	void ComputeBeats(double[][] values) {
		for (int i = 0; i < values.Length - 1; i++) {
			bool isPositive = false;
			bool beatTriggered = false;
			double counter = 0d;
			double negativeCounter = 0d;
			for(int g = 0; g < values[i].Length; g++) {

				if(values[i][g] >= 0 && !beatTriggered) {
					isPositive = true;
					counter += values[i][g];
					if (counter > averages[g]) {
						negativeCounter = 0;
						beatTriggered = true;
						visualMaster.CreateBeat(i * sampleSizeForFFT, values[i][g], g);
						beatCount[g] ++;
					}
				} else if(values[i][g] < 0) {
					isPositive = false;
					beatTriggered = false;
					negativeCounter -= values[i][g];
					counter = 0;
				}
			}		
		}

		for (int i = 0; i < beatCount.Length; i++) {
			Debug.Log("Beats for band " + i.ToString() + " : " + beatCount[i].ToString());		
		}
	}

	void ComputeBeatsAverage(double[][] values) {
		for (int i = 0; i < values.Length - 1; i++) {
			for(int g = 0; g < values[i].Length; g++) {
				if(values[i][g] > averages[g]) {
					visualMaster.CreateBeat(i * sampleSizeForFFT, values[i][g], g);
				}
			}		
		}
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
			Debug.Log("Averages for band " + g.ToString() + " : " + averages[g].ToString());
			averages[g] = averages[g]/counters[g];
			averages[g] *= 1.4f;

		}
	}

	float[] ApplyHamming(float[] values) {
		for (int n = 0; n < values.Length; n++) {
			values[n] = 0.54f - (0.46f * Mathf.Cos( (2f * Mathf.PI * values[n]) / (values.Length - 1) ));	
		}
		return values;
	}
}
