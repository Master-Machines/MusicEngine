using UnityEngine;
using System.Collections;

public class RealtimeVisuals : MonoBehaviour {
	public GameObject audioObj;
	private AudioSource source;
	
	public GameObject[] gridLights;
	private float[] oldValues;
	private float[] currentValues;
	private float[] deltas;
	public GameObject skyParticlesObj;
	private ParticleSystem skyParticles;
	private float[] currentPower;
	private int[] bandCutoffs = new int[4]{64, 128, 192, 256};
	public Color[] colors;
	private int currentColor = 0;
	private int newColor = 0;
	private float songAverage;
	private float starBonus = 0f;
	private TerrainMod tMod;
	public GameObject trail;
	public GameObject[] trailObjects;
	private Trail[] theTrails;
	private float maxAngle = 140f;
	public bool inMenu = false;
	// Use this for initialization
	void Start () {
		if(!inMenu) {
			songAverage = Global.songTotalAverage;
		} else {
			songAverage = .0000001f;
		}
		
		tMod = gameObject.GetComponent<TerrainMod>();
		//Debug.Log(songAverage);
		oldValues = new float[512];
		currentValues = new float[512];
		deltas = new float[512];
		currentPower = new float[4];
		source = audioObj.audio;
		//skyParticles = skyParticlesObj.particleSystem;
		theTrails = new Trail[trailObjects.Length];
		for(int i = 0; i < trailObjects.Length; i++) {
			theTrails[i] = trailObjects[i].GetComponent<Trail>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		ComputeSpectrum();
		ComputePower();
		ChangeStuff();
		starBonus *= .83f;
	}
	
	void ChangeStuff() {
		float counter = 0f;
		
		gridLights[0].light.spotAngle *= .83f;
		gridLights[0].light.spotAngle += (1) * (currentPower[0] + currentPower[1]) * 30f;
		gridLights[1].light.spotAngle *= .83f;
		gridLights[1].light.spotAngle += (4f) * (currentPower[2] + currentPower[3]) * 40f;
		gridLights[0].light.spotAngle --;
		gridLights[1].light.spotAngle --;
		if(gridLights[0].light.spotAngle > maxAngle) {
			gridLights[0].light.spotAngle = maxAngle;
		}
		if(gridLights[1].light.spotAngle > maxAngle) {
			gridLights[1].light.spotAngle = maxAngle;
		}
		for(int i = 0; i < 4; i++) {
			/*gridLights[i].light.spotAngle *= .83f;
			gridLights[i].light.spotAngle += (i + 1) * currentPower[i] * 80f;
			if(gridLights[i].light.spotAngle > 130f) {
				gridLights[i].light.spotAngle = 130f;
			}
			*/
			if(gridLights[0].light.spotAngle < 0) {
				gridLights[0].light.spotAngle = 0;
				//gridLights[i].light.spotAngle *= .83f;
				//gridLights[i].light.spotAngle += (i + 1) * (currentPower[i] + currentPower[i + 1]) * 40f;
				//gridLights[i].light.range *= .82f;
				//gridLights[i].light.range += (i + 1) * currentPower[i] * 200f;
			}
			counter += currentPower[i];
			theTrails[i].height += currentPower[i] * 50f * (i + 1);
			currentPower[i] *= .4f;
				
		}
		/*
		ParticleSystem.Particle[] allParticles = new ParticleSystem.Particle[skyParticles.particleCount];
		skyParticles.GetParticles(allParticles);
		
		Color c = colors[currentColor];
		
		for(int i = 0; i < allParticles.Length; i++) {
		//	allParticles[i].size = allParticles[i].size * .83f;
			//allParticles[i].size = 1f + allParticles[i].size + counter * 2.6f * (i + 1000f) / 1000f * (starBonus + 1f);
			allParticles[i].color = c;
		}
		skyParticles.SetParticles(allParticles, skyParticles.particleCount);
		*/
 	}
 	
 	public void ChangeColors() {
		newColor = Random.Range(0, colors.Length);
		while(newColor == currentColor) {
			newColor = Random.Range(0, colors.Length);
		}
		currentColor = newColor;
 		starBonus = 2f;
 	}
	
	
	void ComputeSpectrum() {
		source.GetSpectrumData(currentValues, 0, FFTWindow.BlackmanHarris);
		for(int i = 0; i < 512; i++) {
			deltas[i] = currentValues[i] - oldValues[i];
			oldValues[i] = currentValues[i];
		}
	}
	
	void ComputePower() {
		for(int i = 0; i < 512; i++) {
			if(deltas[i] > 0) {
				float power = Mathf.Pow(currentValues[i], 1.2f);
				power = (power/ (songAverage * 10000000f))/.5f;
				if(power > .9f) {
					power = .9f;
				}
				if(i > 0 && i < bandCutoffs[0]) {
					currentPower[0] += power;
				} else if(i < bandCutoffs[1]) {
					currentPower[1] += power;
				} else if(i < bandCutoffs[2]) {
					currentPower[2] += power;
				} else if(i < bandCutoffs[3]) {
					currentPower[3] += power;
				}
			}
		}
	}
	
	
}
