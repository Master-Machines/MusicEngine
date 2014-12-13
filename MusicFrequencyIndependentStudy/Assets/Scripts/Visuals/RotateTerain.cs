using UnityEngine;
using System.Collections;

public class RotateTerain : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void MusicStarting() {
		Debug.Log("Rotating");
		iTween.RotateTo (gameObject, new Vector3(0f, 359.999999f, 0f), Global.audioClip.length);
	}
}
