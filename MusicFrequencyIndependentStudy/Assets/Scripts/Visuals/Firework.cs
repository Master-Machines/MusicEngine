using UnityEngine;
using System.Collections;

public class Firework : MonoBehaviour {
	public GameObject[] explosionParticles;
	public GameObject trail;
	public GameObject hitParticles;
	public int hasBeenHit = 0;
	public float timeToExplode;
	public GameObject movementObj;
	public GameObject trailBlue;
	public GameObject successResidualParticles;
	public GameObject failResidualParticles;
	private ScoreTracker scoreTracker;
	// Use this for initialization
	IEnumerator Start () {
		scoreTracker = GameObject.Find("ScoreTracker").GetComponent<ScoreTracker>();
		yield return new WaitForSeconds(timeToExplode);
		gameObject.layer = 2;
		if(hasBeenHit > 0) {
			int numPartles = 2;
			for(int i = 0; i < numPartles; i++) {
				explosionParticles[i].particleSystem.Play();
			}
			Instantiate(successResidualParticles, transform.position, Quaternion.identity);
		} else {
			Instantiate(failResidualParticles, transform.position, Quaternion.identity);
		}
		
		/*
		if (hasBeenHit) {
			int numPartles = 0;
			if(transform.position.y > 30) {
				numPartles = 3;
			} else if(transform.position.y > 0) {
				numPartles = 2;
			} else if(transform.position.y > -20) {
				numPartles = 1;
			}
			numPartles = 1;
			for(int i = 0; i < numPartles; i++) {
				explosionParticles[i].particleSystem.Play();
			}
		} else {
			scoreTracker.Hit();
		}
		*/
	}
	
	// Update is called once per frame
	void Update () {
		if(hasBeenHit > 1) {
			hasBeenHit --;
		}
	}
	
	public void GetHit() {
		hasBeenHit = 10;
	/*
		gameObject.layer = 0;
		trail.particleSystem.Stop();
		trailBlue.particleSystem.Play();
		hitParticles.particleSystem.Play();
		movementObj.GetComponent<BasicMovement>().velocity = new Vector3(Random.Range(-4f, 4f), Random.Range(12f, 20f), Random.Range(-4f, 4f));
		scoreTracker.AddPoints();
		*/
	}
}
