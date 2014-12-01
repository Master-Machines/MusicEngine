using UnityEngine;
using System.Collections;

public class AudioModifier : MonoBehaviour {

	private int regenCounter = -1;
	private AudioLowPassFilter lowPass;
	public int lowestCutoff;
	// Use this for initialization
	void Start () {
		lowPass = gameObject.GetComponent<AudioLowPassFilter>();
	}
	
	// Update is called once per frame
	void Update () {
		if(regenCounter > 0 && lowPass.cutoffFrequency > lowestCutoff) {
			lowPass.cutoffFrequency -= 400;
		} else if(regenCounter > 0) {
			regenCounter --;
		} else if(regenCounter == 0 && lowPass.cutoffFrequency < 22000) {
			lowPass.cutoffFrequency += 600;
			if(lowPass.cutoffFrequency > 21000) {
				lowPass.cutoffFrequency = 22000;
				regenCounter = -1;
			}
		}
	}
	
	public void Damage() {
		if(regenCounter == -1) {
			lowPass.cutoffFrequency = 8000;
		}
		regenCounter = 100;
	}
}
