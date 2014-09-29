using UnityEngine;
using System.Collections;

public class Realtime : MonoBehaviour {
	public int numberOfBands = 16;
	public int numSamples = 1024;
	public FFTWindow fft;
	public GUIStyle gui;
	private float[] spectrum;
	private Texture2D tex;
	// Use this for initialization
	void Start () {
		spectrum = new float[numSamples];
		tex = new Texture2D (1, 1);
		tex.SetPixel (0, 0, Color.red);
		tex.Apply ();
		gui.normal.background = tex;
	}
	
	void Update() {
		audio.GetSpectrumData (spectrum, 0, fft);
		spectrum = FindMagnitudes (spectrum);
	}

	float[] Condense(float[] values, int numBands, bool average) {
		float[] vals = new float[numBands];
		int[] cutoffs = new int[numBands];
		int[] cutoffsAverage = new int[numBands];
		int currentCutoff = 0;
		for(int g = 0; g < numBands; g++) {
			float f = (g + 1f) / (float)numBands;
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

	Rect CreateGUIBox(int position, float value) {
		float xPos = position;
		float yPos = 100f;
		float width = 3f;
		float height = -1 * value - 1;
		return new Rect (xPos, yPos, width, height);
	}

	void OnGUI() {
		for (int i = 0; i < spectrum.Length; i++) {
			GUI.Box(CreateGUIBox(i, spectrum[i] * 100f), "", gui);
		}
	}

	float[] FindMagnitudes(float[] values) {
		for (int i = 0; i < values.Length; i++) {
			values[i] = i * Mathf.Abs(values[i]);
		}
		return values;
	}
}
