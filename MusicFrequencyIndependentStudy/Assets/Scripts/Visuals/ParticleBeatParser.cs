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
	private float xAngle;
	public Vector2 xLimits;
	private float radius = 60f;
	private RealtimeVisuals realtime;
	// Use this for initialization
	void Start () {
		ray = xrayObj.GetComponent<xray>();
		ray2 = xrayObj2.GetComponent<xray>();
		realtime = gameObject.GetComponent<RealtimeVisuals>();
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
		bool reverseXLeft = false;
		bool reverseXRight = false;
		
		bool reverseY = false;
		if(xAngle < xLimits.x ) {
			directions[0].x = 1f;
			reverseX = true;
			reverseXLeft = true;
		} else if(xAngle > xLimits.y) {
			directions[0].x = -1;
			reverseX = true;
			reverseXRight = true;
		}
		if(currentBeatPosition.y + direction.y < centerY - rangeY || currentBeatPosition.y + direction.y > centerY + rangeY) {
			direction.y *= -1f;
			reverseY = true;
		}
		
		if(reverseY) {
			ReverseDirections(reverseX, reverseY);
		}
		Vector3 lastBeatPosition = currentBeatPosition;
		Vector2 newBeats = GetXandZPositionFromAngleAndRadius(xAngle, radius);
		currentBeatPosition = new Vector3(transform.position.x + newBeats.x, currentBeatPosition.y + direction.y, transform.position.z + newBeats.y );
		int strongestMag = GetStrongestMagnitudeIndex(magnitudes);
		GameObject theBeat = (GameObject)Instantiate(linearBeats[strongestMag], currentBeatPosition, Quaternion.identity);
		latestFirework = theBeat.GetComponentInChildren<Firework>();
		
		Vector3 adjustedLastBeatPosition = lastBeatPosition;
		if(reverseXLeft) {
			adjustedLastBeatPosition = new Vector3(-1000f, currentBeatPosition.y, 0f);
		} else if(reverseXRight) {
			adjustedLastBeatPosition = new Vector3(1000f, currentBeatPosition.y, 0f);
		}
		latestFirework.SetInfo(adjustedLastBeatPosition, magnitudes[9], magnitudes[strongestMag], triggeredBeats);
		TweenToPosition[] particleScripts = theBeat.GetComponentsInChildren<TweenToPosition>();
		
		float timeModifier = 1f;
		if(magnitudes[8] < Global.audioClip.frequency * 2) {
			timeModifier = .75f + (float)magnitudes[8] / (float)Global.audioClip.frequency / 8f;
		}
		
		for(int i = 0; i < particleScripts.Length; i++) {
			StartCoroutine(particleScripts[i].StartIt(timeModifier));
		}
		
		if (triggeredBeats > 0) {
			//GameObject g = (GameObject)Instantiate(lineTracker, transform.position, Quaternion.identity);
			//LineTracker lt = g.GetComponent<LineTracker>();
			//StartCoroutine(lt.Instantiate(lastBeatPosition, currentBeatPosition, 1.2f));
		}
		triggeredBeats ++;
		lastBeatPosition = currentBeatPosition;
		StartCoroutine(TriggerRealtime());
		//StartCoroutine(TriggerCrosshair(true));
	}
	
	IEnumerator TriggerRealtime() {
		yield return new WaitForSeconds(primaryDelay / 1000f);
		realtime.ChangeColors();
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
	}
	
	Vector2 GetDirection(float[] magnitudes) {
		int greatestBeatIndex = GetStrongestMagnitudeIndex(magnitudes);
		Vector2 d = directions[greatestBeatIndex];
		
		float total = 3f + magnitudes[8] / 2400f;
		if(total > 22f) {
			total = 22f;
		}
		xAngle += total / 1f * directions[0].x;
		float maxDistance = magnitudes[9] / Global.audioClip.frequency * 10f;
		if(maxDistance > 40f) {
			maxDistance = 40f;
		} 
		radius = 25f + maxDistance;
		return new Vector2(d.x * total, d.y * total);
		
	}
	
	Vector2 GetXandZPositionFromAngleAndRadius(float angle, float r) {
		angle = angle;
		Vector2 v = new Vector2();
		v.y = r * Mathf.Cos(Mathf.Deg2Rad * angle);
		v.x = r * Mathf.Sin(Mathf.Deg2Rad * angle);
		return v;
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
