using UnityEngine;
using System.Collections;

public class RiftReset : MonoBehaviour {
	private GameObject rift;
	private OVRManager manager;
	// Use this for initialization
	void Start () {
		rift = GameObject.Find("OVRCameraRig");
		manager = rift.GetComponent<OVRManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)) {
			OVRManager.display.RecenterPose();
		}
		
	}
}
