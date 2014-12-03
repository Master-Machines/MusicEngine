using UnityEngine;
using System.Collections;

public class MenuObject : MonoBehaviour {

	public GameObject mainParticles;
	public GameObject textObj;
	public int value;
	[HideInInspector]
	public MenuTracker menuTracker;

	private int counter = 0;
	
	// Use this for initialization
	void Start () {
	
	}
	

	void Update () {
		if(counter < 65) {
			counter --;
			if(counter < 0) {
				counter = 0;
			}
			SetParticles();
		}
	}
	
	void SetParticles() {
		float percent = (float)counter / 65f;
		mainParticles.particleSystem.startSpeed = 100f * percent;
	}
	
	public void SetTextAndValue(string text, int v) {
		textObj.GetComponent<TextMesh>().text = text;
		textObj.GetComponent<TextMesh>().fontSize = 40 + (int)((1f - (float)text.Length/44f) * 40f);
		value = v;
	}
	
	public void GetHit() {
		if(counter < 60) {
			counter += 2;
		}
		
		if(counter >= 60 && counter < 64) {
			counter = 65;
			ResetOtherCounters();
			menuTracker.SetValue(value);
			ResetOtherCounters();
		}else if(counter > 10) {
			
		}
		SetParticles();
	}
	
	void ResetOtherCounters() {
		GameObject[] objs = GameObject.FindGameObjectsWithTag("menu");
		foreach(GameObject obj in objs) {
			if(obj != gameObject) {
				obj.GetComponent<MenuObject>().ResetCounter();
			}
		}
	}
	
	public void ResetCounter() {
		counter = 0;
		SetParticles();
	}
}
