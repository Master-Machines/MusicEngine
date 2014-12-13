using UnityEngine;
using System.Collections;

public class Trail : MonoBehaviour {

	public float radius = 200f;
	float timeToComplete = 4f;
	public float time = 0f;
	float currentAngle = 0f;
	Vector3 player;
	public float height;
	private TrailRenderer tRend;
	
	private float maxHeight = 150f;
	// Use this for initialization
	void Start () {
		player = GameObject.Find ("OVRCameraRig").transform.position;
		player.y -= 45f;
		tRend = gameObject.GetComponent<TrailRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
		currentAngle = (1f - (time / timeToComplete)) * 360f;
		float deltaX = radius * Mathf.Cos(Mathf.Deg2Rad * currentAngle);
		float deltaZ = radius * Mathf.Sin(Mathf.Deg2Rad * currentAngle);
		transform.position = new Vector3(player.x + deltaX, player.y + height, player.z + deltaZ);
		tRend.startWidth = 2f + height/20f;
		float timeModifier = 1f;// -currentAngle/360f + .1f;
		if(currentAngle < 150f && currentAngle > 60f) {
			timeModifier = .5f;
		}
		time += Time.deltaTime * timeModifier;
		if(time >= timeToComplete) {
			time = 0;
		}
		height *= .83f;
		if(height > maxHeight) {
			height = maxHeight;
		}
	}
}
