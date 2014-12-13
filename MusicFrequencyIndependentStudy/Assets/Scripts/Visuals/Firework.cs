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
	public GameObject successParticlesSmall;
	public GameObject failResidualParticles;
	public GameObject beatActive;
	public GameObject beatActive2;
	private Vector3 explosionTarget;
	private float timeFromLastBeat;
	private float magnitude;
	private ParticleSystem beatActiveParticles;
	private ParticleSystem beatActiveParticles2;
	private ScoreTracker scoreTracker;
	public bool finished = false;
	private GameObject player;
	public int beatNum = 0;
	public GameObject newLine;
	public GameObject noNewLineExplosion;
	private bool dontShow = true;
	// Use this for initialization
	IEnumerator Start () {
		scoreTracker = GameObject.Find("ScoreTracker").GetComponent<ScoreTracker>();
		AdjustForDifficulty();
		beatActiveParticles = beatActive.particleSystem;
		beatActiveParticles2 = beatActive2.particleSystem;
		player = GameObject.Find ("OVRCameraRig");
		yield return new WaitForSeconds(timeToExplode / 2f);
		dontShow = false;
		yield return new WaitForSeconds((timeToExplode / 2f) - .1f);
		CheckForNextBeat();
		yield return new WaitForSeconds(.1f);
		finished = true;
		gameObject.layer = 2;
		Global.totalBeats ++;
		if(hasBeenHit > 0f || Global.difficulty == 0) {
			/*
			int numPartles = 2;
			for(int i = 0; i < numPartles; i++) {
				explosionParticles[i].particleSystem.Play();
			}*/
			Global.beatsHit ++;
			GameObject dasExplosion = (GameObject)Instantiate(successResidualParticles, transform.position, Quaternion.identity);
			dasExplosion.transform.LookAt(explosionTarget);
			float lifespan = timeFromLastBeat/(float)Global.audioClip.frequency;
			if(lifespan > 2.5f) {
				lifespan = 2.5f;
			}
			dasExplosion.particleSystem.startLifetime = lifespan * 1.1f;
			dasExplosion.particleSystem.Emit(50 + (int)(magnitude * 75f * lifespan));
			lifespan = 3f - lifespan;
			dasExplosion.particleSystem.startSpeed = lifespan * 12f;
			scoreTracker.AddPoints(transform.position);
			
			
			dasExplosion = (GameObject)Instantiate(successParticlesSmall, transform.position, Quaternion.identity);
			dasExplosion.transform.LookAt(explosionTarget);
			lifespan = timeFromLastBeat/(float)Global.audioClip.frequency;
			if(lifespan > 2.5f) {
				lifespan = 2.5f;
			}
			dasExplosion.particleSystem.startLifetime = lifespan * 1.43f;
			dasExplosion.particleSystem.Emit(50 + (int)(magnitude * 65f * lifespan));
			lifespan = 3f - lifespan;
			dasExplosion.particleSystem.startSpeed = lifespan * 12f;
			scoreTracker.AddPoints(transform.position);
		} else {
			Instantiate(failResidualParticles, transform.position, Quaternion.identity);
			AudioModifier audioModifier = GameObject.Find ("VisualMaster").GetComponent<AudioModifier>();
			audioModifier.Damage();
			scoreTracker.Hit();
		}
	}
	
	void CheckForLastBeat() {
		GameObject[] fireworks = GameObject.FindGameObjectsWithTag("firework");
		bool notFound = true;
		for(int i = 0; i < fireworks.Length; i++) {
			Firework f = fireworks[i].GetComponent<Firework>();
			if(f != null && f.beatNum == beatNum - 1 && !f.finished) {
				notFound = false;
			}
		}
		if(notFound) {
			Instantiate(noNewLineExplosion, transform.position, Quaternion.identity);
			Debug.Log ("created ripple");
		}
	}
	
	void CheckForNextBeat() {
		GameObject[] fireworks = GameObject.FindGameObjectsWithTag("firework");
		for(int i = 0; i < fireworks.Length; i++) {
			Firework f = fireworks[i].GetComponent<Firework>();
			if(f != null && f.beatNum == beatNum + 1) {
				GameObject l = (GameObject)Instantiate(newLine, transform.position, Quaternion.identity);
				float distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(fireworks[i].transform.position.x, fireworks[i].transform.position.z));
				float time = .35f;
				Hashtable h = new Hashtable();
				h.Add("time", time);
				h.Add("easing", iTween.EaseType.linear);
				h.Add("position", fireworks[i].transform.position);
				Destroy(l, 1f);
				iTween.MoveTo(l, h);
			}
		}
	}
	
	void AdjustForDifficulty() {
		BoxCollider b = gameObject.GetComponent<BoxCollider>();
		if(Global.difficulty == 1) {
			float modifier = 1.5f;
			b.size = new Vector3(b.size.x * modifier, b.size.y * modifier, b.size.z * modifier);
		} else {
			float modifier = 1.25f;
			b.size = new Vector3(b.size.x * modifier, b.size.y * modifier, b.size.z * modifier);
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
		if(!beatActiveParticles.isPlaying && !dontShow) {
			beatActiveParticles.Play();
			beatActiveParticles2.Play();
		}
	}
	
	public void SetInfo(Vector3 expTarget, float timeToNextBeat, float magnitudeOfBeat, int beatNumber) {
		explosionTarget = expTarget;
		magnitude = magnitudeOfBeat;
		timeFromLastBeat = timeToNextBeat;
		beatNum = beatNumber;
		CheckForLastBeat();
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
