using UnityEngine;
using System.Collections;

public class LookAtTarget : MonoBehaviour {
	public GameObject target;
	private int counter = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(++counter == 20) {
			gameObject.transform.LookAt(target.transform);
		}
	}
}
