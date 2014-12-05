using UnityEngine;
using System.Collections;

public class PostGame : MonoBehaviour {
	public GameObject textTotal;
	public GameObject textHit;
	public GameObject textPercent;
	public GameObject textStreak;
	
	// Use this for initialization
	IEnumerator Start () {
		textTotal.GetComponent<TextMesh>().text = "Total Beats: " + Global.totalBeats;
		textHit.GetComponent<TextMesh>().text = "You Hit: " + Global.beatsHit + " (" + GetPercentHit() + ")";
		textStreak.GetComponent<TextMesh>().text = "Longest Streak: " + Global.longestStreak;
		yield return new WaitForSeconds(7f);
		Application.LoadLevel(0);
	}
	
	string GetPercentHit() {
		float percent = ((float)Global.beatsHit / (float)Global.totalBeats) * 100f;
		string pString = ((int)percent).ToString();
		return pString + "%";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
