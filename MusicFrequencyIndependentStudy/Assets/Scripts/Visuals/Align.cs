using UnityEngine;
using System.Collections;

public class Align : MonoBehaviour {
	public GameObject alignObj;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)) {
			transform.LookAt(alignObj.transform);
		}
	}
}
