using UnityEngine;
using System.Collections;

public class LineMover : MonoBehaviour {

	private Vector3 velocity;
	private bool ready = false;
	
	public IEnumerator Instantiate(float lifespan) {
		yield return new WaitForSeconds(lifespan);
		Destroy(gameObject);
	}
	
	void Update () {
	}
}
