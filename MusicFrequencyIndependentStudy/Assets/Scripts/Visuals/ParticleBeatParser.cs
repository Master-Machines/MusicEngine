using UnityEngine;
using System.Collections;

public class ParticleBeatParser : MonoBehaviour {
	public GameObject beatParticles;

	public GameObject delayedBeatParticles;
	
	public GameObject crazyDelayedBeatParticles;
	public GameObject secondaryCrazyDelayedBeatParticles;
	public Vector2 creationPlane;
	public float primaryDelay;
	public GameObject xrayObj;
	public GameObject xrayObj2;
	public GameObject gridLight;
	public Color[] gridFlashColors;
	private xray ray;
	private xray ray2;
	// Use this for initialization
	void Start () {
		ray = xrayObj.GetComponent<xray>();
		ray2 = xrayObj2.GetComponent<xray>();
	}
	
	// Update is called once per frame
	void Update () {
		gridLight.light.range *= .92f;
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
		GameObject createdBeat;
		if(mag == 2f) {
			if(Time.time > 5f) {
				createdBeat = (GameObject)Instantiate (crazyDelayedBeatParticles, new Vector3 (Random.Range(-1f * creationPlane.x, creationPlane.x), 0f, Random.Range(-1f * creationPlane.y, creationPlane.y)), Quaternion.identity);
			}
			StartCoroutine(TriggerCrosshair(true));
		} else {
			StartCoroutine(TriggerCrosshair(false));
			// createdBeat = (GameObject)Instantiate (secondaryCrazyDelayedBeatParticles, new Vector3 (Random.Range(-1f * creationPlane.x, creationPlane.x), 0f, Random.Range(-1f * creationPlane.y, creationPlane.y)), Quaternion.identity);
		}
		/*float baseRadius = .2f;
		float radiusMultiplier = 180f;
		createdBeat.transform.localScale = new Vector3 (baseRadius + radiusMultiplier * mag, baseRadius + radiusMultiplier * mag, baseRadius + radiusMultiplier * mag);
		*/
	}
	
	private IEnumerator TriggerCrosshair(bool isBig) {
		yield return new WaitForSeconds(primaryDelay / 1000f);
		StartCoroutine (ray.TurnOn());
		StartCoroutine (ray2.TurnOn());
		
		//iTween.ColorTo(gridLight, Color.black, .25f);
		if(isBig) {
			gridLight.light.range += 700f;
			iTween.Stop(gridLight);
			iTween.ColorTo(gridLight, gridFlashColors[Random.Range(0, gridFlashColors.Length - 1)], .2f);
		} else {
			gridLight.light.range += 300f;
		}
		
	}
}
