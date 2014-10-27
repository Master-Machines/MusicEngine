using UnityEngine;
using System.Collections;

public class ParticleBeatParser : MonoBehaviour {
	public GameObject beatParticles;

	public GameObject delayedBeatParticles;
	
	public GameObject crazyDelayedBeatParticles;
	public Vector2 creationPlane;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void TriggerParticleBeat(float[] options) {
		int band = (int)options [0];
		float mag = options [1];
		GameObject createdBeat = (GameObject)Instantiate (beatParticles, new Vector3 (band * 5f, 0f, 0f), Quaternion.identity);
		//createdBeat.particleSystem.startLifetime = beatLength [beatIndex] * .1f + .6f;
		//createdBeat.particleSystem.startColor = new Color (1f, 1f, 1f, beatMagnitude[beatIndex] * 5f);
		//createdBeat.particleSystem.time = 0f;
		//createdBeat.particleSystem.Play ();
	}


	public void DelayedTriggerParticleBeat(float[] options) {
		int band = (int)options [0];
		float mag = options [1];
		GameObject createdBeat = (GameObject)Instantiate (delayedBeatParticles, new Vector3 (band * 5f, 0f, 10f), Quaternion.identity);
		float baseRadius = .2f;
		float radiusMultiplier = 180f;
		createdBeat.transform.localScale = new Vector3 (baseRadius + radiusMultiplier * mag, baseRadius + radiusMultiplier * mag, baseRadius + radiusMultiplier * mag);
		//createdBeat.particleSystem.startLifetime = beatLength [beatIndex] * .1f + .6f;
		//createdBeat.particleSystem.startColor = new Color (1f, 1f, 1f, beatMagnitude[beatIndex] * 5f);
		//createdBeat.particleSystem.time = 0f;
		//createdBeat.particleSystem.Play ();
	}
	
	public void CrazyDelayedTriggerParticleBeat(float[] options) {
		int band = (int)options [0];
		float mag = options [1];
		GameObject createdBeat = (GameObject)Instantiate (crazyDelayedBeatParticles, new Vector3 (Random.Range(-1f * creationPlane.x, creationPlane.x), 0f, Random.Range(-1f * creationPlane.y, creationPlane.y)), Quaternion.identity);
		float baseRadius = .2f;
		float radiusMultiplier = 180f;
		createdBeat.transform.localScale = new Vector3 (baseRadius + radiusMultiplier * mag, baseRadius + radiusMultiplier * mag, baseRadius + radiusMultiplier * mag);
		//createdBeat.particleSystem.startLifetime = beatLength [beatIndex] * .1f + .6f;
		//createdBeat.particleSystem.startColor = new Color (1f, 1f, 1f, beatMagnitude[beatIndex] * 5f);
		//createdBeat.particleSystem.time = 0f;
		//createdBeat.particleSystem.Play ();
	}
}
