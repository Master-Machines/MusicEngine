using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	private string currentDifficulty = "Medium";
	private string currentSong;
	private string currentBeatDetection = "";
	public AudioClip[] audioClips;
	// Use this for initialization
	void Start () {
		SetDifficulty(3);
		SetSong(5);
		SetAudioMods(0);
		Application.LoadLevel(1);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void SetDifficulty(int difficulty) {
		switch (difficulty) {
			case 0:
				Global.minTimeBetweenBeats = 40000;
				currentDifficulty = "Easy";
			break;
			case 1:
				Global.minTimeBetweenBeats = 30000;
				currentDifficulty = "Medium";
			break;
			case 2:
				Global.minTimeBetweenBeats = 16000;
				currentDifficulty = "Hard";
			break;
			case 3:
				Global.minTimeBetweenBeats = 7000;
				currentDifficulty = "Pure";
			break;
		}
	}
	
	void SetAudioMods(int mod) {
		switch (mod) {
			case 0:
			// Standard
			Global.audioModifiers = new float[8]{1.3f, 1.2f, 1.1f, 1f, .9f, .8f, .7f, .7f};
			currentBeatDetection = "Balanced";
			break;
			case 1:
			// High treble
			Global.audioModifiers = new float[8]{4f, 4f, 2f, 1f, 0f, 0f, 0f, 0f};
			currentBeatDetection = "Highs";
			break;
			case 2:
			// High treble and high mids
			Global.audioModifiers = new float[8]{2f, 2f, 2f, 2f, .75f, 0f, 0f, 0f};
			currentBeatDetection = "Highs and mids";
			break;
			case 3:
			// High base
			Global.audioModifiers = new float[8]{0f, 0f, 0f, 0f, 1f, 1.2f, 1.4f, 1.6f};
			currentBeatDetection = "Base";
			break;
			case 4:
			// High mids
			Global.audioModifiers = new float[8]{0f, .7f, 3f, 3f, .7f, 0f, 0f, 0f};
			currentBeatDetection = "Mids";
			break;
		}
	}
	
	void SetSong(int song) {
		currentSong = audioClips[song].name;
		Global.audioClip = audioClips[song];
	}
	
	void OnGUI() {
		GUI.Label(new Rect(Screen.width * .2f, Screen.height * .1f, 300f, 300f), "Current Difficulty: " + currentDifficulty);
		
		if(GUI.Button(new Rect(Screen.width * .2f, 100f, 150f, 70f), "Easy")) {
			SetDifficulty(0);
		} else if(GUI.Button(new Rect(Screen.width * .2f, 200f, 150f, 70f), "Medium")) {
			SetDifficulty(1);
		} else if(GUI.Button(new Rect(Screen.width * .2f, 300f, 150f, 70f), "Hard")) {
			SetDifficulty(2);
		} else if(GUI.Button(new Rect(Screen.width * .2f, 400f, 150f, 70f), "Pure")) {
			SetDifficulty(3);
		}
		
		GUI.Label(new Rect(Screen.width * .4f, Screen.height * .1f, 300f, 300f), "Beat Detection Focus: " + currentBeatDetection);
		
		if(GUI.Button(new Rect(Screen.width * .4f, 100f, 150f, 65f), "Balanced")) {
			SetAudioMods(0);
		} else if(GUI.Button(new Rect(Screen.width * .4f, 170f, 150f, 65f), "Highs")) {
			SetAudioMods(1);
		} else if(GUI.Button(new Rect(Screen.width * .4f, 240f, 150f, 65f), "Highs and mids")) {
			SetAudioMods(2);
		} else if(GUI.Button(new Rect(Screen.width * .4f, 310f, 150f, 65f), "Mids")) {
			SetAudioMods(4);
		} else if(GUI.Button(new Rect(Screen.width * .4f, 380f, 150f, 65f), "Base")) {
			SetAudioMods(3);
		}
		
		GUI.Label(new Rect(Screen.width * .7f, Screen.height * .1f, 200, 200), "Current Song: " + currentSong);
		
		for(int i = 0; i < audioClips.Length; i++) {
			if(GUI.Button(new Rect(Screen.width * .7f, Screen.height * .1f * (i + 2), Screen.width * .29f, Screen.height * .09f), audioClips[i].name)) {
				SetSong(i);
			}
		}
		
		if(GUI.Button(new Rect(Screen.width * .4f, Screen.height - 150f, Screen.width * .2f, 120f), "Begin Game")) {
			Application.LoadLevel(1);
		}
	}
}
