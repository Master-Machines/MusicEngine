using UnityEngine;
using System.Collections;
using System.IO;

public class MenuTracker : MonoBehaviour {

	private int currentValues = 0;
	private string currentDifficulty = "Medium";
	private string currentSong;
	private string currentBeatDetection = "";
	public AudioClip[] audioClips;
	public GameObject menuObject;
	public GameObject nextObject;
	public GameObject menuControlsSmall;
	public GameObject menuControlsSmall2;
	public GameObject menuObjectDirectory;
	private bool nextCreated;
	private string[] difficultyLabels = new string[5]{"Visualizer", "Easy", "Medium", "Hard", "Pure"};
	private MusicImport mImport;
	
	private string currentDrive = "";
	private string[] currentDrives;
	private DirectoryInfo currentDirectory;
	private DirectoryInfo[] currentDirectories;
	private int dirIndex = 0;
	private FileInfo[] importSongs;
	private int importSongIndex;
	// Use this for initialization
	void Start () {
		mImport = gameObject.GetComponent<MusicImport>();
		SetupSongLabels();
		SetAudioMods(0);
		ResetGlobalValues();
		if(Global.lastDirectory != null) {
			currentDirectory = Global.lastDirectory;
			currentDrive = Global.lastDrive;
		}
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
		DestroyMenuObjects();
		CreateMenuObjects(sLabels, 0f, menuObject, 0);
		
		GameObject n = (GameObject)Instantiate(menuObject, new Vector3(transform.position.x, transform.position.y + 80f, transform.position.z + 180f), Quaternion.identity);
		n.GetComponent<MenuObject>().SetTextAndValue("Import Music", -2);
		n.GetComponent<MenuObject>().menuTracker = this;
	}
	
	void SetupDifficultyLabels() {
		DestroyMenuObjects();
		CreateMenuObjects(difficultyLabels, 0f, menuObject, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void CreateMenuObjects(string[] labels, float verticalModifier, GameObject obj, int baseIndex) {
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
			GameObject mObj = (GameObject)Instantiate(obj, new Vector3(transform.position.x + xModifier, transform.position.y + verticalModifier, transform.position.z + zModifier), Quaternion.identity);
			mObj.GetComponent<MenuObject>().SetTextAndValue(labels[i], baseIndex + songCounter);
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
		if(i == -1) {
			// Next button hit
			Destroy(GameObject.FindGameObjectWithTag("menuNext"));
			nextCreated = false;
			currentValues ++;
			if(currentValues == 0) {
				SetupSongLabels();
			} else if(currentValues == 1 || currentValues == 4) {
				SetupDifficultyLabels();
				currentValues = 1;
			} else if(currentValues == 2) {
				if(currentDirectory != null) {
					Global.lastDirectory = currentDirectory;
					Global.lastDrive = currentDrive;
				}
				Application.LoadLevel(1);
			}
		} else if(i == -2) {
			GameObject m = GameObject.FindGameObjectWithTag("menuNext");
			if(m != null) {
				Destroy(m);
				nextCreated = false;
			}
			if(currentDirectory == null) {
				currentValues = 2;
			} else {
				importSongs = MusicImport.FindMusicInDirectory(currentDirectory);
				currentValues = 3;
			}
			// Load Imported Music Menu
			DestroyMenuObjects();
			CreateImportMenu();
		} else if(i == -3) {
			dirIndex --;
			DestroyMenuObjects();
			CreateImportMenu();
		} else if(i == -4) {
			dirIndex ++;
			DestroyMenuObjects();
			CreateImportMenu();
		} else if(i == -5){
			if(currentDirectory != null) {
				if(currentDirectory.Parent == null) {
					currentValues = 2;
					DestroyMenuObjects();
					currentDirectory = null;
					currentDrive = "";
					CreateImportMenu();
				} else {
					currentDirectory = currentDirectory.Parent;
					importSongs = new FileInfo[0];
					DestroyMenuObjects();
					CreateImportMenu();
				}
			} else {
				currentValues = 2;
				DestroyMenuObjects();
				currentDirectory = null;
				currentDrive = "";
				CreateImportMenu();
			}
		} else if(i == -6) {
			importSongIndex --;
			DestroyMenuObjects();
			CreateImportMenu();
		} else if(i == -7) {
			importSongIndex ++;
			DestroyMenuObjects();
			CreateImportMenu();
		} else if(i == -8) {
			currentValues = 0;
			DestroyMenuObjects();
			currentDirectory = null;
			currentDrive = "";
			SetupSongLabels();
		} else if(i < -100) {
			i += 110;
			StartCoroutine(MusicImport.ImportAudio(importSongs[i + importSongIndex]));
			
		}else if(currentValues == 2) {
			// Set Imported Music
			currentDrive = currentDrives[i];
			DestroyMenuObjects();
			CreateImportMenu();
			currentValues = 3;
		} else if(currentValues == 3) {
			dirIndex = 0;
			currentDirectory = currentDirectories[i];
			importSongs = MusicImport.FindMusicInDirectory(currentDirectory);
			DestroyMenuObjects();
			CreateImportMenu();
		}else if(currentValues == 0) {
			// Set default music
			SetSong(i);
		} else if(currentValues == 1) {
			SetDifficulty(i);
		}
		
		if(i > -1 && !nextCreated && currentValues != 3) {
			nextCreated = true;
			GameObject n = (GameObject)Instantiate(nextObject, new Vector3(transform.position.x, transform.position.y - 100f, transform.position.z + 150f), Quaternion.identity);
			n.GetComponent<MenuObject>().SetTextAndValue("Next", -1);
			n.GetComponent<MenuObject>().menuTracker = this;
		}
	}
	
	public void ImportComplete() {
		audio.clip = Global.audioClip;
		audio.Play();
		if(!nextCreated) {
			nextCreated = true;
			GameObject n = (GameObject)Instantiate(nextObject, new Vector3(transform.position.x, transform.position.y - 100f, transform.position.z + 150f), Quaternion.identity);
			n.GetComponent<MenuObject>().SetTextAndValue("Next", -1);
			n.GetComponent<MenuObject>().menuTracker = this;
		}
	}
	
	void CreateImportMenu() {
		CreateDriveView();
		if(currentDirectory != null) {
			CreateDirectoryControls();
		}
		
		if(importSongs != null) {
			CreateImportSongList();
			CreateSongImportControls();
		}
		
	}
	
	void CreateImportSongList() {
		if(importSongs.Length > 0) {
			int max = 8;
			if(max > importSongs.Length) {
				max = importSongs.Length;
			}
			string[] songsToDisplay = new string[max];
			for(int i = 0; i < max; i++) {
				songsToDisplay[i] = importSongs[i + importSongIndex].Name;
			}
			CreateMenuObjects(songsToDisplay, -40f, menuObject, -110);
		}
	}
	
	void CreateSongImportControls() {
		if(importSongIndex > 0) {
			GameObject left = (GameObject)Instantiate(menuControlsSmall2, new Vector3(transform.position.x - 50, transform.position.y - 60f, transform.position.z + 150f), Quaternion.identity);
			left.GetComponent<MenuObject>().SetTextAndValue("<--", -6);
			left.GetComponent<MenuObject>().menuTracker = this;
		}
		
		if(importSongIndex + 8 < importSongs.Length) {
			GameObject right = (GameObject)Instantiate(menuControlsSmall2, new Vector3(transform.position.x + 50, transform.position.y - 60f, transform.position.z + 150f), Quaternion.identity);
			right.GetComponent<MenuObject>().SetTextAndValue("-->", -7);
			right.GetComponent<MenuObject>().menuTracker = this;
		}
	}
	
	void CreateDirectoryControls() {
		if(dirIndex > 0) {
			GameObject left = (GameObject)Instantiate(menuControlsSmall, new Vector3(transform.position.x - 50, transform.position.y + 10f, transform.position.z + 150f), Quaternion.identity);
			left.GetComponent<MenuObject>().SetTextAndValue("<--", -3);
			left.GetComponent<MenuObject>().menuTracker = this;
		}
		
		if(dirIndex + 8 < currentDirectories.Length) {
			GameObject right = (GameObject)Instantiate(menuControlsSmall, new Vector3(transform.position.x + 50, transform.position.y + 10f, transform.position.z + 150f), Quaternion.identity);
			right.GetComponent<MenuObject>().SetTextAndValue("-->", -4);
			right.GetComponent<MenuObject>().menuTracker = this;
		}
		
		GameObject back = (GameObject)Instantiate(menuControlsSmall, new Vector3(transform.position.x, transform.position.y + 10f, transform.position.z + 175f), Quaternion.identity);
		back.GetComponent<MenuObject>().SetTextAndValue("Back", -5);
		back.GetComponent<MenuObject>().menuTracker = this;
	}
	
	
	void CreateDriveView() { 
		if(currentDrive == "") {
			currentDrives = MusicImport.GetLogicalDrives();
			CreateMenuObjects(currentDrives, 60f, menuObjectDirectory, 0);
		} else if (currentDirectory == null){
			currentDirectory = new DirectoryInfo(currentDrive);
			currentDirectories = MusicImport.GetSubDirectories(currentDirectory);
			CreateMenuObjects(directoriesToStrings(currentDirectories), 60f, menuObjectDirectory, dirIndex);
		} else {
			currentDirectories = MusicImport.GetSubDirectories(currentDirectory);
			CreateMenuObjects(directoriesToStrings(currentDirectories), 60f, menuObjectDirectory, dirIndex);
		}
		
		GameObject back = (GameObject)Instantiate(menuControlsSmall, new Vector3(transform.position.x, transform.position.y + 10f, transform.position.z + 175f), Quaternion.identity);
		back.GetComponent<MenuObject>().SetTextAndValue("Back", -8);
		back.GetComponent<MenuObject>().menuTracker = this;
		
	}
	
	string[] directoriesToStrings(DirectoryInfo[] ds) {
		int max = 8;
		if(ds.Length < 8) {
			max = ds.Length;
		}
		string[] ss = new string[max];
		int counter = 0;
		for(int i = dirIndex; i < dirIndex + max; i++) {
			ss[counter] = ds[i].Name;
			counter ++;
		}
		return ss;
	}
	
	void SetDifficulty(int difficulty) {
		Global.difficulty = difficulty;
		switch (difficulty) {
		case 0: 
			Global.minTimeBetweenBeats = 7000;
			break;
		case 1:
			Global.minTimeBetweenBeats = 40000;
			currentDifficulty = "Easy";
			break;
		case 2:
			Global.minTimeBetweenBeats = 30000;
			currentDifficulty = "Medium";
			break;
		case 3:
			Global.minTimeBetweenBeats = 16000;
			currentDifficulty = "Hard";
			break;
		case 4:
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
