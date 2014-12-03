using UnityEngine;
using System.Collections;

public class MenuXRay : MonoBehaviour {
	public LayerMask layerMask;
	
	// Update is called once per frame
	void Update () {
		LaserIt();
	}
	
	void LaserIt() {
		Vector3 dirToCrosshair = camera.transform.forward.normalized;
		Ray ray = new Ray(gameObject.transform.position, dirToCrosshair);
		RaycastHit hit;
		//Debug.DrawRay(camera.transform.position, dirToCrosshair * 100f);
		if(Physics.Raycast(ray, out hit, 1000f)) {
			MenuObject f = hit.collider.gameObject.GetComponent<MenuObject>();
			if(f != null) {
				f.GetHit();
			}
			
		}
	}
}
