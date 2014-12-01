using UnityEngine;
using System.Collections;

public class LineTracker : MonoBehaviour {
	
	public GameObject lineObj;

	private Vector3 startingPosition;
	private Vector3 endingPosition;
	private bool ready = false;
	private int counter = 3;
	private int maxCounter = 7;

	public IEnumerator Instantiate(Vector3 sPos, Vector3 ePos, float lifeSpan) {
		startingPosition = sPos;
		endingPosition = ePos;
		ready = true;
		yield return new WaitForSeconds(lifeSpan);
		Destroy(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		if(ready && counter++ > maxCounter) {
			counter = 0;
			CreateLineObj();	
		}
	}
	
	void CreateLineObj() {
		GameObject g = (GameObject)Instantiate(lineObj, startingPosition, Quaternion.identity);
		iTween.MoveTo(g, endingPosition, 1f);
	}
}
