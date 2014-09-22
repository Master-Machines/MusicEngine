using UnityEngine;
using System.Collections;

public class VisualMaster : MonoBehaviour {
	public int gridCount;
	public Vector3 startingPosition;
	public float rowXChangeAmount;
	public float rowZChangeAmount;

	public float cellXChangeAmount;
	public float cellZChangeAmount;

	public GameObject colorBox;
	public GameObject beatCollectorObj;
	public GameObject beat;
	private float zPosition;
	private float startingRowZChangeAmount;
	// Use this for initialization
	void Start () {
		startingRowZChangeAmount = rowZChangeAmount;
	}
	
	// Update is called once per frame
	void Update () {
		if (audio.isPlaying) beatCollectorObj.BroadcastMessage ("CheckIfTime", audio.timeSamples);
	}

	public void CreateRow(double[] values) {
		rowZChangeAmount = startingRowZChangeAmount;// * GetTotalEnergy(values) * .001f;
		gridCount ++;
		zPosition += rowZChangeAmount;
		//Debug.Log ((gridCount * rowZChangeAmount) - 10f);
		//iTween.MoveTo (Camera.main.gameObject, new Vector3 (0f, 0f, (gridCount * rowZChangeAmount) - 10f), 0f);
		Camera.main.transform.position = new Vector3 (values.Length * .01f, 1f, zPosition - values.Length * .02f);
		for (int i = 0; i < values.Length/2; i++) {
			GameObject obj = (GameObject) Instantiate(colorBox, new Vector3(0, 0, 0), Quaternion.identity);
			ColorBox cBox = obj.GetComponent<ColorBox>();
			cBox.Setup(new Vector3(gridCount * rowXChangeAmount + i * cellXChangeAmount, 0f, zPosition + i * cellZChangeAmount), values[i], gridCount);
		}
	}

	float GetTotalEnergy (double[] values) {
		double counter = 0;
		for (int i = 0; i < values.Length/2; i++) {
			if(values[i] > 0) {
				counter += values[i];
			}

		}
		return (float)counter;
	}

	public void CreateBeat(int time, double magnitude, int band) {
		GameObject beatObj = (GameObject)Instantiate (beat, new Vector3 (band * 5f, 0f, 0f), Quaternion.identity);
		beatObj.transform.parent = beatCollectorObj.transform;
		Beat beatScript = beatObj.GetComponent<Beat> ();
		beatScript.sampleTime = time;
		beatScript.band = band;
		beatScript.magnitude = (float) magnitude;
	}
}
