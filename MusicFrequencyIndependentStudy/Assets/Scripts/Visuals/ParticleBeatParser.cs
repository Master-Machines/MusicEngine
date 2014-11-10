using UnityEngine;
using System.Collections;

public class ParticleBeatParser : MonoBehaviour {

	public GameObject linearBeat;
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
	// Use this for initialization
	void Start () {
		ray = xrayObj.GetComponent<xray>();
		ray2 = xrayObj2.GetComponent<xray>();
		currentBeatPosition = new Vector3(centerX, centerY, 0f);
	}
	
	// Update is called once per frame
	void Update () {
		gridLight.light.range *= .92f;
	}
	
	public void CreateLinearBeat(float[] magnitudes) {
		Vector2 direction = new Vector2(0f, 0f);
		float distanceModifier = .5f;
		for(int i = 0; i < magnitudes.Length; i++) {
			direction.x += directions[i].x * magnitudes[i] * distanceModifier;
			direction.y +=  directions[i].y * magnitudes[i] * distanceModifier;
		}
		direction = ModifyDirection(direction);
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
		currentBeatPosition = new Vector3(currentBeatPosition.x + direction.x, currentBeatPosition.y + direction.y, Mathf.Abs(currentBeatPosition.x + direction.x)/ -2f );
		Instantiate(linearBeat, currentBeatPosition, Quaternion.identity);
		
		if (triggeredBeats > 0) {
			GameObject g = (GameObject)Instantiate(lineTracker, transform.position, Quaternion.identity);
			LineTracker lt = g.GetComponent<LineTracker>();
			StartCoroutine(lt.Instantiate(lastBeatPosition, currentBeatPosition, 7f));
		}
		triggeredBeats ++;
		lastBeatPosition = currentBeatPosition;
		StartCoroutine(TriggerCrosshair(true));
	}
	
	void ReverseDirections(bool x, bool y) {
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
	
	Vector2 ModifyDirection(Vector2 d) {
		float total = Mathf.Abs(d.x) + Mathf.Abs(d.y);
		if(total > 14f) {
			total = 14f;
		}
		d.Normalize();
		return new Vector2(d.x * (5f + total), d.y * (5f + total));
		
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
