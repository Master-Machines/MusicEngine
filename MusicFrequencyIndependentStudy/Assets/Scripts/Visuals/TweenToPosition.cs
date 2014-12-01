using UnityEngine;
using System.Collections;

public class TweenToPosition : MonoBehaviour {
	public Vector3 localPositionToMoveTo;
	public float time;
	public iTween.EaseType easeType;
	// Use this for initialization
	public IEnumerator StartIt (float delayModifier) {
		yield return new WaitForSeconds(time * (1f - delayModifier));
		time = time * delayModifier;
		easeType = iTween.EaseType.easeInCubic;
		Hashtable hash = new Hashtable();
		hash.Add("oncomplete", "AnimationFinished");
		hash.Add("time", time);
		hash.Add("isLocal", true);
		hash.Add("position", localPositionToMoveTo);
		hash.Add("easetype", easeType);
		iTween.MoveTo(gameObject, hash);
		
		transform.localScale = new Vector3(delayModifier * delayModifier * delayModifier * delayModifier * 10f, 3f, 3f);
		easeType = iTween.EaseType.easeOutCubic;
		hash = new Hashtable();
		hash.Add("oncomplete", "AnimationFinished");
		hash.Add("time", time);
		hash.Add("isLocal", true);
		hash.Add("scale", new Vector3(delayModifier * delayModifier * .5f, 1f, 1f));
		hash.Add("easetype", easeType);
		iTween.ScaleTo(gameObject, hash);
		particleSystem.startSize *= delayModifier;
		particleSystem.Play();
	}
	
	void AnimationFinished() {
		Destroy(gameObject);
	}
}
