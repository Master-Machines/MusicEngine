using UnityEngine;
using System.Collections;

public class ScoreTracker : MonoBehaviour {

	public GameObject pointsObj;
	private TextMesh pointsMesh;
	public GameObject multObj;
	private TextMesh multMesh;
	
	private int points = 0;
	private int multiplier = 1;
	// Use this for initialization
	void Start () {
		pointsMesh = pointsObj.GetComponent<TextMesh>();
		multMesh = multObj.GetComponent<TextMesh>();
		pointsMesh.text = "0";
		multMesh.text = "1x";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void AddPoints() {
		points += multiplier;
		pointsMesh.text = points.ToString();
		multiplier ++;
		multMesh.text = multiplier.ToString() + "x";
	}
	
	public void Hit() {
		multiplier /= 2;
		if(multiplier < 1) {
			multiplier = 1;
		}
		multMesh.text = multiplier.ToString() + "x";
	}
}
