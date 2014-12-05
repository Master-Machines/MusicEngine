using UnityEngine;
using System.Collections;

public class MenuTracker : MonoBehaviour {

	private int currentValues = 0;
	private string currentDifficulty = "Medium";
	private string currentSong;
	private string currentBeatDetection = "";
	public AudioClip[] audioClips;
	public GameObject menuObject;
	public GameObject nextObject;
	private bool nextCreated;
	private string[] difficultyLabels = new string[4]{"Easy", "Medium", "Hard", "Pure"};
	// Use this for initialization
	void Start () {
		SetupSongLabels();
		SetAudioMods(0);
		ResetGlobalValues();
	}
	
	void ResetGlobalValues() {
		Global.beatsHit = 0;
		Global.totalBeats = 0;
		Global.longestStreak = 0;
	}
	
	void SetupSongLabels() {
		string[] sLabels = new string[audioClips.Length];
		for(int i = 0; i < audioClips.Length; i++) {
			if(audioClips[i].name.Length > 44) {
				sLabels[i] = audioClips[i].name.Substring(0, 41) + "...";
			} else {
				sLabels[i] = audioClips[i].name;
			}
			
		}
		CreateMenuObjects(sLabels);
	}
	
	void SetupDifficultyLabels() {
		Debug.Log("anoit");
		
		CreateMenuObjects(difficultyLabels);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void CreateMenuObjects(string[] labels) {
		DestroyMenuObjects();
		Debug.Log("test");
		int total = labels.Length;
		int startingValue = 0;
		if(total % 2 == 0) {
			startingValue = -1 * total / 2;
		} else {
			startingValue = (-1 * (total - 1) / 2);
		}
		
		float radius = 250f;
		float degreeChange = 23f;
		int songCounter = 0;
		for(int i = 0; i < labels.Length; i++) {
			float degree = 90f - (degreeChange * startingValue);
			float zModifier = radius * Mathf.Sin(Mathf.Deg2Rad * degree);
			float xModifier = radius * Mathf.Cos(Mathf.Deg2Rad * degree);
			GameObject mObj = (GameObject)Instantiate(menuObject, new Vector3(transform.position.x + xModifier, transform.position.y, transform.position.z + zModifier), Quaternion.identity);
			Debug.Log(labels[i]);
			mObj.GetComponent<MenuObject>().SetTextAndValue(labels[i], songCounter);
			songCounter ++;
			mObj.GetComponent<MenuObject>().menuTracker = this;
			startingValue ++;
		}
	}
	
	void DestroyMenuObjects() {
		GameObject[] objs = GameObject.FindGameObjectsWithTag("menu");
		for(int i = 0; i <  objs.Length; i++) {
			Destroy(objs[i]);
		}
		
	}
	
	public void SetValue(int i) {
		Debug.Log("tesdt");
		if(i == -1) {
			// Next button hit
			Destroy(GameObject.FindGameObjectWithTag("menuNext"));
			nextCreated = false;
			currentValues ++;
			
			if(currentValues == 0) {
				SetupSongLabels();
			} else if(currentValues == 1) {
				SetupDifficultyLabels();
			} else if(currentValues == 2) {
				Application.LoadLevel(1);
			}
		} else if(currentValues == 0) {
			SetSong(i);
		} else if(currentValues == 1) {
			SetDifficulty(i);
		}
		
		if(i > -1 && !nextCreated) {
			nextCreated = true;
			GameObject n = (GameObject)Instantiate(nextObject, new Vector3(transform.position.x, transform.position.y - 100f, transform.position.z + 150f), Quaternion.identity);
			n.GetComponent<MenuObject>().SetTextAndValue("Next", -1);
			n.GetComponent<MenuObject>().menuTracker = this;
		}
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
			Global.audioModifiers = new float[8]{1, 1, 1, 1, 1, 1, 1, 1};
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
		audio.clip = Global.audioClip;
		audio.Play();
	}
}
