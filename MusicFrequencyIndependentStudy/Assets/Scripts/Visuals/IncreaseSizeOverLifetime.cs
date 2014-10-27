using UnityEngine;
using System.Collections;

public class IncreaseSizeOverLifetime : MonoBehaviour {
	public float linearIncrease;
	public float percentIncrease;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float newScale = transform.localScale.x + ((transform.localScale.x * percentIncrease) + linearIncrease) * Time.deltaTime;
		
		transform.localScale = new Vector3(newScale, newScale, newScale);
	}
}
