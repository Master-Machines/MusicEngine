using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreTracker : MonoBehaviour {

	public GameObject pointsObj;
	private TextMesh pointsMesh;
	public GameObject multObj;
	private TextMesh multMesh;
	
	private int points = 0;
	private int multiplier = 1;
	
	private int currentCombo = 0;
	private float comboTimer = 0;
	private List<Vector3> comboPath = new List<Vector3>();
	public GameObject comboText;
	public GameObject lineTrackerCombo;
	public GameObject comboTextSpawnPosition;
	private int startingFontSize;
	private TextMesh currentComboTextMesh;
	// Use this for initialization
	void Start () {
		pointsMesh = pointsObj.GetComponent<TextMesh>();
		multMesh = multObj.GetComponent<TextMesh>();
		pointsMesh.text = "0";
		multMesh.text = "1x";
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void EndCombo() {
		if(currentCombo >= 4) {
			Destroy(currentComboTextMesh.gameObject);
			/*
			for(int i = 0; i < comboPath.Count - 1; i++) {
				GameObject lt = (GameObject)Instantiate(lineTrackerCombo, transform.position, Quaternion.identity);
				StartCoroutine (lt.GetComponent<LineTracker>().Instantiate(comboPath[i], comboPath[i + 1], 4f));
				if(i > 0) {
					lt = (GameObject)Instantiate(lineTrackerCombo, transform.position, Quaternion.identity);
					StartCoroutine (lt.GetComponent<LineTracker>().Instantiate(comboPath[i], comboPath[i - 1], 4f));
				}
			}
			*/
			//points += (int)Mathf.Pow ((float)currentCombo, 1.1f);
			int newPoints = currentCombo;
			if(currentCombo >= 100) {
				newPoints = currentCombo * 3;
			} else if(currentCombo >= 75) {
				newPoints = (int)(currentCombo * 2.5f);
			} else if(currentCombo >= 50) {
				newPoints = currentCombo * 2;
			} else if(currentCombo >= 25) {
				newPoints = (int)(currentCombo * 1.5f);
			}
			points += newPoints;
			pointsMesh.text = points.ToString();
		}
		comboPath.Clear();
		currentCombo = 0;
	}
	
	public void AddPoints(Vector3 position) {
		points += multiplier;
		pointsMesh.text = points.ToString();
		multMesh.text = multiplier.ToString() + "x";
		currentCombo ++;
		comboTimer = 1.4f;
		comboPath.Add (position);
		
		if(currentCombo == 4) {
			GameObject text = (GameObject)Instantiate(comboText, comboTextSpawnPosition.transform.position, Quaternion.identity);
			text.transform.parent = comboTextSpawnPosition.transform.parent.transform;
			currentComboTextMesh = text.GetComponent<TextMesh>();
			startingFontSize = currentComboTextMesh.fontSize;
			currentComboTextMesh.text = "" + currentCombo.ToString() + "x";
		} else if(currentCombo > 4) {
			currentComboTextMesh.text = "" + currentCombo.ToString() + "x";
			StartCoroutine(IncreaseFontSize());
		}
	}
	
	IEnumerator IncreaseFontSize() {
		int oldFontSize = startingFontSize;
		int extraFont = (currentCombo);
		if(extraFont > 50) {
			extraFont = 50;
		}
		currentComboTextMesh.fontSize += 1 + extraFont;
		currentComboTextMesh.text = currentCombo.ToString() + "x!";
		yield return new WaitForSeconds(.3f);
		if(currentComboTextMesh != null) {
			currentComboTextMesh.fontSize = oldFontSize;
			currentComboTextMesh.text = currentCombo.ToString() + "x";
		}
		
	}
	
	public void Hit() {
		multiplier /= 2;
		if(multiplier < 1) {
			multiplier = 1;
		}
		EndCombo();
		multMesh.text = multiplier.ToString() + "x";
	}
}
