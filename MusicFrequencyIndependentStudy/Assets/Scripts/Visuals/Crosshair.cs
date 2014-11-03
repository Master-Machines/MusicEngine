using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {
	public GameObject leftCam;
	public GameObject rightCam;
	public float zOffset;
	// Use this for initialization
	IEnumerator Start () {
		yield return new WaitForSeconds(.1f);
		Vector3 newPosition = new Vector3();
		newPosition.x = (leftCam.transform.localPosition.x - rightCam.transform.localPosition.x) / -2f;
		newPosition.y = (leftCam.transform.localPosition.y - rightCam.transform.localPosition.y) / -2f;
		newPosition.z = (leftCam.transform.localPosition.z - rightCam.transform.localPosition.z) / -2f + zOffset;
		transform.localPosition = newPosition;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
