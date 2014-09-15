using UnityEngine;
using System.Collections;

public class ColorBox : MonoBehaviour {
	private float magnitude;
	public int gridPosition;
	int deathCounter = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (deathCounter++ == 3) {
			Destroy(gameObject);		
		}
	}

	public void Setup (Vector3 pos, double mag, int gridPos) {
		magnitude = (float)mag;
		if ((magnitude) < .01f) {
			Destroy(gameObject);		
		}
		gridPosition = gridPos;
		transform.position = new Vector3(pos.x, pos.y + magnitude/2f, pos.z);
		transform.localScale = new Vector3(.2f, Mathf.Abs(magnitude), .2f);
		//StartCoroutine (Kill (1f));
	}

	IEnumerator Kill(float timeToDeath) {
		yield return new WaitForSeconds (timeToDeath);
		Destroy (gameObject);
	}
}
