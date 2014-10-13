using UnityEngine;
using System.Collections;

public class Realtime : MonoBehaviour {
	public int numberOfBands = 16;
	public int numSamples = 1024;
	public FFTWindow fft;
	public GUIStyle gui;
	public GameObject coreScriptObject;
	public bool displayCondensedValues = true;
	private Core core;
	private float[] spectrum;
	private Texture2D tex;

	private Texture2D tex2;
	// Use this for initialization
	void Start () {
		core = coreScriptObject.GetComponent<Core> ();
		spectrum = new float[numSamples];
		tex = new Texture2D (1, 1);
		tex.SetPixel (0, 0, Color.red);
		tex.Apply ();

		tex2 = new Texture2D (1, 1);
		tex2.SetPixel (0, 0, Color.blue);
		tex2.Apply ();
		gui.normal.background = tex;
	}
	
	void Update() {
		audio.GetSpectrumData (spectrum, 0, fft);
		spectrum = FindMagnitudes (spectrum);
	}

	int GetSongSampleIndex() {
		int time = audio.timeSamples;
		return (int)(time / core.sampleSizeForFFT);
	}

	Rect CreateGUIBox(int position, float value) {
		float xPos = position;
		float yPos = 100;
		float width = 3f;
		float height = -1 * value - 1;
		return new Rect (xPos, yPos, width, height);
	}

	Rect CreateGUIBoxDelta(int position, float value, float width) {
		float xPos = position;
		float yPos = 200;
		float height = -1 * value;
		return new Rect (xPos, yPos, width, height);
	}

	void OnGUI() {
		gui.normal.background = tex;
		for (int i = 0; i < spectrum.Length; i++) {
			GUI.Box(CreateGUIBox(i, spectrum[i] * 100f), "", gui);
		}

		if (displayCondensedValues) {
			int index = GetSongSampleIndex();
			for(int i = 0; i < core.deltas[index].Length; i++) {
				int xPos = 0;
				float width = (float)spectrum.Length * core.bandPercents[i];
				if(i > 0) {
					xPos = (int)(spectrum.Length * core.bandPercents[i - 1]);
					width -= (float)spectrum.Length * core.bandPercents[i-1];
				}
				if(core.deltas[index][i] < 0) gui.normal.background = tex;
				else gui.normal.background = tex2;
				GUI.Box(CreateGUIBoxDelta(xPos, (float)core.deltas[index][i] * 8000000f, width), "", gui);

				gui.normal.background = tex2;
				GUI.Box(new Rect((float)xPos, 250f, 1f, -250f), "", gui);
			}
		}
	}

	float[] FindMagnitudes(float[] values) {
		for (int i = 0; i < values.Length; i++) {
			values[i] = i * Mathf.Abs(values[i]);
		}
		return values;
	}
}
