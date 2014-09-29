using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BeatMaster : MonoBehaviour {

	public GameObject beatParticles;
	
	private List<int> beatBand = new List<int>();
	private List<float> beatMagnitude = new List<float>();
	private List<int> beatLength = new List<int>();
	private List<int> beatTime = new List<int>();
	private List<bool> beatTriggered = new List<bool>();

	public int audioFrequency;
	public int sampleSize;
	public float sampleRate;
	private int totalBeats = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		CheckForBeats ();
	}

	void CheckForBeats() {
		int currentTime = audio.timeSamples;
		for (int i = 0; i < totalBeats; i++) {
			if( !beatTriggered[i] && beatTime[i] < currentTime + 6000) {
				beatTriggered[i] = true;
				StartCoroutine(TriggerBeat(currentTime, i));
			}
		}
	}

	IEnumerator TriggerBeat(int currentTime, int beatIndex) {
		yield return new WaitForSeconds ((sampleRate) * (beatTime [beatIndex] - currentTime));
		GameObject createdBeat = (GameObject)Instantiate (beatParticles, new Vector3 (beatBand[beatIndex] * 5f, 0f, 0f), Quaternion.identity);
		createdBeat.particleSystem.startLifetime = beatLength [beatIndex] * .1f + .6f;
		createdBeat.particleSystem.startColor = new Color (1f, 1f, 1f, beatMagnitude[beatIndex] * 5f);
		createdBeat.particleSystem.time = 0f;
		createdBeat.particleSystem.Play ();
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
}
