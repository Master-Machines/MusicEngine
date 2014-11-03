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
	private ScoreTracker scoreTracker;
	// Use this for initialization
	IEnumerator Start () {
		scoreTracker = GameObject.Find("ScoreTracker").GetComponent<ScoreTracker>();
		yield return new WaitForSeconds(timeToExplode);
		if (hasBeenHit) {
			int numPartles = 0;
			if(transform.position.y > 30) {
				numPartles = 3;
			} else if(transform.position.y > 0) {
				numPartles = 2;
			} else if(transform.position.y > -20) {
				numPartles = 1;
			}
			for(int i = 0; i < numPartles; i++) {
				explosionParticles[i].particleSystem.Play();
			}
		} else {
			scoreTracker.Hit();
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
			movementObj.GetComponent<BasicMovement>().velocity = new Vector3(Random.Range(-4f, 4f), Random.Range(12f, 20f), Random.Range(-4f, 4f));
			scoreTracker.AddPoints();
		}
		
	}
}
