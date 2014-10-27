using UnityEngine;
using System.Collections;

public class BasicMovement : MonoBehaviour {
	public Vector3 velocity;
	
	public bool isRandom;
	public Vector3 minRandomVelocity;
	public Vector3 maxRandomVelocity;
	// Use this for initialization
	void Start () {
		if(isRandom) {
			velocity = new Vector3(Random.Range(minRandomVelocity.x, maxRandomVelocity.x), Random.Range(minRandomVelocity.y, maxRandomVelocity.y),Random.Range(minRandomVelocity.z, maxRandomVelocity.z));
		}
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (velocity * Time.deltaTime);
	}
}
