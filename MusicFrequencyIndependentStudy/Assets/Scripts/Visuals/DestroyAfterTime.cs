using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour {
	public float time;
	// Use this for initialization
	void Start () {
		StartCoroutine (DestroyObjectAfterTime ());
	}

	IEnumerator DestroyObjectAfterTime() {
		yield return new WaitForSeconds(time);
		Destroy(gameObject);
	}
}
