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
	private float songAverage;
	// Use this for initialization
	void Start () {
		songAverage = Global.songTotalAverage;
		//Debug.Log(songAverage);
		oldValues = new float[512];
		currentValues = new float[512];
		deltas = new float[512];
		currentPower = new float[4];
		source = audioObj.audio;
		skyParticles = skyParticlesObj.particleSystem;
	}
	
	// Update is called once per frame
	void Update () {
		ComputeSpectrum();
		ComputePower();
		ChangeStuff();
	}
	
	void ChangeStuff() {
		float counter = 0f;
		for(int i = 0; i < 4; i++) {
			gridLights[i].light.spotAngle *= .83f;
			gridLights[i].light.spotAngle += (i + 1) * currentPower[i] * 80f;
			if(gridLights[i].light.spotAngle > 130f) {
				gridLights[i].light.spotAngle = 130f;
			}
			counter += currentPower[i];
			currentPower[i] *= .4f;
			
		}
		ParticleSystem.Particle[] allParticles = new ParticleSystem.Particle[skyParticles.particleCount];
		skyParticles.GetParticles(allParticles);
		for(int i = 0; i < allParticles.Length; i++) {
			allParticles[i].size = allParticles[i].size * .83f;
			allParticles[i].size = 1f + allParticles[i].size + counter * 2.6f;
		}
		skyParticles.SetParticles(allParticles, skyParticles.particleCount);
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
				float power = Mathf.Pow(currentValues[i], 1f);
				power = power/3f + (power/ (songAverage * 10000000f))/2f;
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
