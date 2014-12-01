using UnityEngine;
using System.Collections;

public class ParticleBeatParser : MonoBehaviour {

	public GameObject[] linearBeats;
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
	private Vector3 currentBeatPosition;
	public Vector2[] directions;
	public float centerX;
	public float rangeX;
	public float centerY;
	public float rangeY;
	public GameObject lineTracker;
	private Vector3 lastBeatPosition;
	private int triggeredBeats = 0;
	private Firework latestFirework;
	// Use this for initialization
	void Start () {
		ray = xrayObj.GetComponent<xray>();
		ray2 = xrayObj2.GetComponent<xray>();
		currentBeatPosition = new Vector3(centerX, centerY, 0f);
	}
	
	// Update is called once per frame
	void Update () {
		//gridLight.light.range *= .92f;
	}
	
	public void CreateLinearBeat(float[] magnitudes) {
		Vector2 direction = new Vector2(0f, 0f);
		direction = GetDirection(magnitudes);
		bool reverseX = false;
		bool reverseY = false;
		if(currentBeatPosition.x + direction.x  < centerX - rangeX || currentBeatPosition.x + direction.x > centerX + rangeX) {
			direction.x *= -1f;
			reverseX = true;
		}
		if(currentBeatPosition.y + direction.y < centerY - rangeY || currentBeatPosition.y + direction.y > centerY + rangeY) {
			direction.y *= -1f;
			reverseY = true;
		}
		
		if(reverseX || reverseY) {
			ReverseDirections(reverseX, reverseY);
		}
		Vector3 lastBeatPosition = currentBeatPosition;
		currentBeatPosition = new Vector3(currentBeatPosition.x + direction.x, currentBeatPosition.y + direction.y, Mathf.Abs(currentBeatPosition.x + direction.x)/ -2f );
		int strongestMag = GetStrongestMagnitudeIndex(magnitudes);
		GameObject theBeat = (GameObject)Instantiate(linearBeats[strongestMag], currentBeatPosition, Quaternion.identity);
		latestFirework = theBeat.GetComponentInChildren<Firework>();
		latestFirework.SetInfo(lastBeatPosition, magnitudes[8], magnitudes[strongestMag]);
		TweenToPosition[] particleScripts = theBeat.GetComponentsInChildren<TweenToPosition>();
		
		float timeModifier = 1f;
		if(magnitudes[8] < Global.audioClip.frequency * 2) {
			timeModifier = .75f + (float)magnitudes[8] / (float)Global.audioClip.frequency / 8f;
		}
		
		for(int i = 0; i < particleScripts.Length; i++) {
			StartCoroutine(particleScripts[i].StartIt(timeModifier));
		}
		
		if (triggeredBeats > 0) {
			GameObject g = (GameObject)Instantiate(lineTracker, transform.position, Quaternion.identity);
			LineTracker lt = g.GetComponent<LineTracker>();
			StartCoroutine(lt.Instantiate(lastBeatPosition, currentBeatPosition, 1f));
		}
		triggeredBeats ++;
		lastBeatPosition = currentBeatPosition;
		//StartCoroutine(TriggerCrosshair(true));
	}
	
	void ReverseDirections(bool x, bool y) {
		if(x && latestFirework) {
			latestFirework.DirectionChange();
		}
		for(int i = 0; i < directions.Length; i++) {
			if(x) {
				directions[i].x *= -1f;		
			}
			if(y) {
				directions[i].y *= -1f;
			}
		}
		Debug.Log("Direction reversed");
	}
	
	Vector2 GetDirection(float[] magnitudes) {
		int greatestBeatIndex = GetStrongestMagnitudeIndex(magnitudes);
		Vector2 d = directions[greatestBeatIndex];
		
		float total = 3f + magnitudes[8] / 2400f;
		if(total > 22f) {
			total = 22f;
		}
		return new Vector2(d.x * total, d.y * total);
		
	}
	
	int GetStrongestMagnitudeIndex(float[] magnitudes) {
		int greatestBeatIndex = 0;
		float greatestBeatMag = 0f;
		for(int i = 0; i < 8; i++) {
			if(magnitudes[i] > greatestBeatMag) {
				greatestBeatIndex = i;
				greatestBeatMag = magnitudes[i];
			}
		}
		return greatestBeatIndex;
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
