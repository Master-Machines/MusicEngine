using UnityEngine;
using System.Collections;

public class Firework : MonoBehaviour {
	public GameObject[] explosionParticles;
	public GameObject trail;
	public GameObject hitParticles;
	public bool hasBeenHit = false;
	public float timeToExplode;
	public GameObject movementObj;
	public GameObject trailBlue;
	// Use this for initialization
	IEnumerator Start () {
		yield return new WaitForSeconds(timeToExplode);
		if (hasBeenHit) {
			for(int i = 0; i < explosionParticles.Length - 1; i++) {
				explosionParticles[i].particleSystem.Play();
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void GetHit() {
		if(!hasBeenHit) {
			hasBeenHit = true;
			gameObject.layer = 0;
			trail.particleSystem.Stop();
			trailBlue.particleSystem.Play();
			hitParticles.particleSystem.Play();
			movementObj.GetComponent<BasicMovement>().velocity = new Vector3(0f, 17f, 0f);
		}
		
	}
}
