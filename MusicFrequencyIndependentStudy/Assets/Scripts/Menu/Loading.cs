using UnityEngine;
using System.Collections;

public class Loading : MonoBehaviour {

	// Use this for initialization
	IEnumerator Start () {
		yield return new WaitForSeconds(.1f);
		Application.LoadLevel(2);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
