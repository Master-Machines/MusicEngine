using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	private string currentDifficulty = "Medium";
	private string currentSong;
	public AudioClip[] audioClips;
	// Use this for initialization
	void Start () {
		SetDifficulty(1);
		SetSong(0);
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
				Global.minTimeBetweenBeats = 20000;
				currentDifficulty = "Hard";
			break;
			case 3:
				Global.minTimeBetweenBeats = 0;
				currentDifficulty = "Pure";
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
		
		GUI.Label(new Rect(Screen.width * .8f, Screen.height * .1f, 200, 200), "Current Song: " + currentSong);
		
		for(int i = 0; i < audioClips.Length; i++) {
			if(GUI.Button(new Rect(Screen.width * .8f, Screen.height * .1f * (i + 2), Screen.width * .19f, Screen.height * .09f), audioClips[i].name)) {
				SetSong(i);
			}
		}
		
		if(GUI.Button(new Rect(Screen.width * .4f, Screen.height - 200f, Screen.width * .2f, 150f), "Begin Game")) {
			Application.LoadLevel(1);
		}
	}
}
