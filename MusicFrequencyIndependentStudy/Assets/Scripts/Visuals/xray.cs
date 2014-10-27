using UnityEngine;
using System.Collections;

public class xray : MonoBehaviour {
	public LayerMask layerMask;
	public GameObject crosshairObj;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		LaserIt();
	}
	
	void LaserIt() {
		Vector3 dirToCrosshair = (crosshairObj.transform.position - transform.position).normalized;
		Ray ray = new Ray(gameObject.transform.position, dirToCrosshair);
		RaycastHit hit;
		//Debug.DrawRay(gameObject.transform.position, dirToCrosshair * 100f);
		if(Physics.Raycast(ray, out hit, 1000f)) {
			Firework f = hit.collider.gameObject.GetComponent<Firework>();
			if(f != null) {
				f.GetHit();
			}
			
		}
	}
}
