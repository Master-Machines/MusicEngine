using UnityEngine;
using System.Collections;

public class Firework : MonoBehaviour {
	public GameObject[] explosionParticles;
	public GameObject trail;
	public GameObject hitParticles;
	public float hasBeenHit = 0;
	public float timeToExplode;
	public GameObject movementObj;
	public GameObject trailBlue;
	public GameObject successResidualParticles;
	public GameObject failResidualParticles;
	public GameObject beatActive;
	public GameObject beatActive2;
	private Vector3 explosionTarget;
	private float timeFromLastBeat;
	private float magnitude;
	private ParticleSystem beatActiveParticles;
	private ParticleSystem beatActiveParticles2;
	private ScoreTracker scoreTracker;
	private bool finished = false;
	// Use this for initialization
	IEnumerator Start () {
		scoreTracker = GameObject.Find("ScoreTracker").GetComponent<ScoreTracker>();
		beatActiveParticles = beatActive.particleSystem;
		beatActiveParticles2 = beatActive2.particleSystem;
		yield return new WaitForSeconds(timeToExplode);
		finished = true;
		gameObject.layer = 2;
		Global.totalBeats ++;
		if(hasBeenHit > 0f) {
			/*
			int numPartles = 2;
			for(int i = 0; i < numPartles; i++) {
				explosionParticles[i].particleSystem.Play();
			}*/
			Global.beatsHit ++;
			GameObject dasExplosion = (GameObject)Instantiate(successResidualParticles, transform.position, Quaternion.identity);
			dasExplosion.transform.LookAt(explosionTarget);
			float lifespan = timeFromLastBeat/44100f + .15f;
			if(lifespan > 2.5f) {
				lifespan = 2.5f;
			}
			dasExplosion.particleSystem.startLifetime = lifespan;
			lifespan = 3f - lifespan;
			dasExplosion.particleSystem.startSpeed = lifespan * 10f + 10f;
			dasExplosion.particleSystem.Emit(50 + (int)(magnitude * 500f));
			scoreTracker.AddPoints(transform.position);
		} else {
			Instantiate(failResidualParticles, transform.position, Quaternion.identity);
			AudioModifier audioModifier = GameObject.Find ("VisualMaster").GetComponent<AudioModifier>();
			audioModifier.Damage();
			scoreTracker.Hit();
		}
	}
	
	void Update () {
		if(hasBeenHit > 0f && !finished) {
			hasBeenHit -= Time.deltaTime;
			if(hasBeenHit < .2f) {
				if(beatActiveParticles.isPlaying) {
					beatActiveParticles.Stop();
					beatActiveParticles2.Stop();
				}
			}
		} else {
			if(beatActiveParticles.isPlaying) {
				beatActiveParticles.Stop();
				beatActiveParticles2.Stop();
			}
		}
	}
	
	public void GetHit() {
		hasBeenHit = .45f;
		if(!beatActiveParticles.isPlaying) {
			beatActiveParticles.Play();
			beatActiveParticles2.Play();
		}
	}
	
	public void SetInfo(Vector3 expTarget, float timeToNextBeat, float magnitudeOfBeat) {
		explosionTarget = expTarget;
		magnitude = magnitudeOfBeat;
		timeFromLastBeat = timeToNextBeat;
	}
	
	public void DirectionChange() {
		float deltaX = transform.position.x - explosionTarget.x;
		if(deltaX >= transform.position.x) {
			explosionTarget = new Vector3(transform.position.x - 10, transform.position.y, transform.position.z);
		} else {
			explosionTarget = new Vector3(transform.position.x + 10, transform.position.y, transform.position.z);
		}
		
	}
}
