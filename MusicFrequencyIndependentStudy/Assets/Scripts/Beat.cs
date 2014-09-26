using UnityEngine;
using System.Collections;

public class Beat : MonoBehaviour {
	public int sampleTime;
	public float magnitude;
	public int band;
	public GameObject particles;
	private bool triggered = false;
	private int sampleThreshold = 200;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void CheckIfTime(int sTime) {

		if (!triggered && sampleTime > (sTime - 2 * sampleThreshold) && sampleTime < (sTime + sampleThreshold)) {
		 	Trigger();			
		}
	}

	void Trigger() {
		triggered = true;
		GameObject p = (GameObject)Instantiate (particles, transform.position, Quaternion.identity);
		//p.particleSystem.startSize = magnitude * .03f;
		//yield return new WaitForSeconds (1f);
		//Destroy (p);
		Destroy (gameObject);
	}
}
