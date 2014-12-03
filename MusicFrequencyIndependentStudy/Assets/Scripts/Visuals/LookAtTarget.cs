using UnityEngine;
using System.Collections;

public class LookAtTarget : MonoBehaviour {
	public bool useName;
	public string name;
	public GameObject target;
	private int counter = 16;
	// Use this for initialization
	void Start () {
		if(useName) {
			target = GameObject.Find(name);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(++counter == 20) {
			gameObject.transform.LookAt(target.transform);
		}
	}
}
