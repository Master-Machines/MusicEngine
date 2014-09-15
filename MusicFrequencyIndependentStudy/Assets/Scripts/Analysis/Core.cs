using UnityEngine;
using System.Collections;
using MathNet.Numerics.IntegralTransforms;
using SpeedTest;

public class Core : MonoBehaviour {
	public AudioSource audioSource;
	public AudioClip audioClip;
	public GameObject visualMasterObject;
	public int sampleSizeFactorOf2;
	private int sampleSizeForFFT;
	private VisualMaster visualMaster;
	// Use this for initialization
	void Start () {
		sampleSizeForFFT = (int)Mathf.Pow (2f, (float)sampleSizeFactorOf2);
		visualMaster = visualMasterObject.GetComponent<VisualMaster> ();
		Debug.Log (audioClip.samples);
		Debug.Log (audioClip.channels);
		Debug.Log (audioClip.frequency);
		StartCoroutine (DoManyIterations (audioClip.samples/sampleSizeForFFT, sampleSizeForFFT, true));
		// StartCoroutine (DoManyIterations (100, sampleSizeForFFT, false));
	}
	
	// Update is called once per frame
	void Update () {

	}

	IEnumerator DoManyIterations (int numberOfIterations, int sampleSize, bool timeToSong) {
		float refreshRate = 1f / (audioClip.frequency / sampleSizeForFFT);
		Debug.Log ("Refresh Rate Peroid: " + refreshRate.ToString ());
		if (!timeToSong) {	
			refreshRate = .05f;
		}

		for(int i = 0; i < numberOfIterations; i++) {
			yield return new WaitForSeconds(refreshRate);
			DoOneIteration(sampleSize, i);
		}
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
		visualMaster.CreateRow (real);
	}

	double[] FindMagnitudes(double[] values) {
		for (int i = 0; i < values.Length/2; i++) {
			values[i] = i * values[i];
		}
		return values;
	}
}
