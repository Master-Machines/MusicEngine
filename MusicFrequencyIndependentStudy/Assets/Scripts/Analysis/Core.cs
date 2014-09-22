using UnityEngine;
using System.Collections;
using MathNet.Numerics.IntegralTransforms;
using SpeedTest;

public class Core : MonoBehaviour {
	public AudioSource audioSource;
	public AudioClip audioClip;
	public GameObject visualMasterObject;
	public int sampleSizeFactorOf2;
	public double beatThreshold;
	public int numberOfBands;
	private int sampleSizeForFFT;
	private VisualMaster visualMaster;
	private double[][] condensedValues;
	private double[][] deltas;
	private float[] averages;
	// Use this for initialization
	void Start () {
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
		double[] imaginary = new double[numberOfSamples];
		double[] real = new double[numberOfSamples];
		for (int i = 0; i < numberOfSamples; i++) {
			imaginary[i] = 0f;
			real[i] = (double)samples[i];
		}
		FFT2 fft = new FFT2();
		fft.init((uint)sampleSizeFactorOf2);
		fft.run (real, imaginary, false);
		real = FindMagnitudes (real);
		condensedValues [offset] = Condense (real, numberOfBands, false);
		//visualMaster.CreateRow (real);
	}

	double[] FindMagnitudes(double[] values) {
		for (int i = 0; i < values.Length/2; i++) {
			values[i] = i * values[i];
		}
		return values;
	}

	double[] Condense(double[] values, int numBands, bool average) {
		double[] vals = new double[numBands];
		int[] cutoffs = new int[numBands];
		int currentCutoff = 0;
		for(int g = 0; g < numBands; g++) {
			float f = (float)(g + 1) / (float)numBands;
			cutoffs[g] = (int)(f * (float)values.Length);
		}
		for (int i = 0; i < values.Length; i++) {
			if(i == cutoffs[currentCutoff]) {
				currentCutoff ++;
			}
			vals[currentCutoff] += values[i];
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
				if(values[i][g] > 0) {
					averages[g] += (float)values[i][g];
					counters[g] += 1;
				}

			}
		}

		for(int g = 0; g < averages.Length; g++) {
			averages[g] = averages[g]/counters[g];
			averages[g] *= 1.4f;
		}
	}
}
