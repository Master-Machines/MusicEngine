using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BeatMaster : MonoBehaviour {

	public int primaryDelay = 0;
	public string parserMethodName;


	private List<int> beatBand = new List<int>();
	private List<float> beatMagnitude = new List<float>();
	private List<int> beatLength = new List<int>();
	private List<int> beatTime = new List<int>();
	private List<bool> beatTriggered = new List<bool>();
	private int visualDelay = 190;
	[HideInInspector]
	public int audioFrequency;
	[HideInInspector]
	public int sampleSize;
	[HideInInspector]
	public float sampleRate;
	private int totalBeats = 0;

	private float[] power;
	private int powerTimeIncrement;
	private int currentPowerIndex;
	private Texture2D tex;
	[HideInInspector]
	public GUIStyle gui;

	private Texture2D tex2;
	[HideInInspector]
	public GUIStyle gui2;
	// Use this for initialization
	void Start () {
		tex = new Texture2D (1, 1);
		tex.SetPixel (0, 0, new Color(0f, 0f, 1f, .6f));
		tex.Apply ();
		gui.normal.background = tex;

		tex2 = new Texture2D (1, 1);
		tex2.SetPixel (0, 0, new Color(0f, 1f, 0f, .8f));
		tex2.Apply ();
		gui2.normal.background = tex2;
	}
	
	// Update is called once per frame
	void Update () {
		CheckForBeats ();
		currentPowerIndex = audio.timeSamples / powerTimeIncrement;
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			visualDelay += 5;
		} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
			visualDelay ++;	
		} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
			visualDelay -= 5;
		} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			visualDelay --;	
		}
	}

	void SetBeatDelays() {
		int samplesDelay = ConvertMSToSamples (primaryDelay);
		for (int i = 0; i < beatTime.Count; i++) {
			beatTime[i] = beatTime[i] - samplesDelay;
			if(beatTime[i] < 0) {
				beatTriggered[i] = true;
			}
		}
	
	}

	int ConvertMSToSamples(int samples) {
		return (int)(((float)audioFrequency) * (float)samples / 1000f);
	}


	void CheckForBeats() {
		int currentTime = audio.timeSamples;
		for (int i = 0; i < totalBeats; i++) {
			if( !beatTriggered[i] && beatTime[i] < currentTime + 10000) {
				beatTriggered[i] = true;
				StartCoroutine(TriggerBeat(currentTime, i));
			}
		}
	}

	IEnumerator TriggerBeat(int currentTime, int beatIndex) {
		yield return new WaitForSeconds ((sampleRate) * (beatTime [beatIndex] - currentTime) + (visualDelay * 0.001f));
		gameObject.SendMessage (parserMethodName, new float[2]{(float)beatBand[beatIndex], beatMagnitude[beatIndex]});
	}

	// Time is starting time in samples, length is length of the beat in samples
	public void CreateBeat(int time, int length, int band, float mag) {
		beatTime.Add (time);
		beatLength.Add (length);
		beatMagnitude.Add (mag);
		beatBand.Add (band);
		beatTriggered.Add (false);
		totalBeats ++;
	}

	public void CalculatePower(int numberOfSamples, int sampleIncrement, int sampleRange) {
		SetBeatDelays ();
		power = new float[numberOfSamples / sampleIncrement];
		powerTimeIncrement = sampleIncrement;
		int bTime = 0;
		float bMag = 0f;
		int pIndex = 0;
		for (int i = 0; i < beatTime.Count; i++) {
			bTime = beatTime[i];
			bMag = beatMagnitude[i];
			pIndex = bTime / sampleIncrement;
			for(int g = pIndex - sampleRange/2; g < pIndex + sampleRange; g++) {
				if(g > 0 && g < power.Length) {
					power[g] += bMag;
				}
			}
		}
		CalculatePowerAverage ();
	}

	void CalculatePowerAverage() {
		float average = 0f;
		float max = 0f;
		for (int i = 0; i < power.Length; i++) {
			average += power[i];
			if(power[i] > max) {
				max = power[i];
			}
		}
		average = average / power.Length;
		average = Mathf.Pow (average, 1f / 5f);
		for (int i = 0; i < power.Length; i++) {
			power[i] = power[i] / max;
			power[i] = power[i] * average;
		}
	}

	void OnGUI() {
		GUI.Box (new Rect (10, Screen.height - 40, 200, 50), "Visual delay: " + visualDelay + "ms");
//		int xPos = Screen.width - 300;
//		int xPosOrig = xPos;
//		int yPos = Screen.height;
//		if (power.Length > 0) {
//			for (int i = currentPowerIndex; i < currentPowerIndex + 300; i++) {
//				GUI.Box(new Rect(xPos++, yPos, 1, -1 * power[i] * 100), "", gui);
//			}
//			GUI.Box(new Rect(xPosOrig, yPos, 2, -1 * 20), "", gui2);
//		}
	} 
}
