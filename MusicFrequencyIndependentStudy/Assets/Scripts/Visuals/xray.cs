﻿using UnityEngine;
using System.Collections;

public class xray : MonoBehaviour {
	public LayerMask layerMask;
	public GameObject redCrosshairObj;
	public GameObject greenCrosshairObj;
	private bool canShoot = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(canShoot) {
			LaserIt();
		}
		
	}
	
	void LaserIt() {
		Vector3 dirToCrosshair = (redCrosshairObj.transform.position - transform.position).normalized;
		Ray ray = new Ray(gameObject.transform.position, dirToCrosshair);
		RaycastHit hit;
		Debug.DrawRay(gameObject.transform.position, dirToCrosshair * 100f);
		if(Physics.Raycast(ray, out hit, 1000f)) {
			Firework f = hit.collider.gameObject.GetComponent<Firework>();
			if(f != null) {
				f.GetHit();
			}
			
		}
	}
	
	public IEnumerator TurnOn() {
		canShoot = true;
		redCrosshairObj.particleSystem.Clear();
		greenCrosshairObj.particleSystem.Play();
		redCrosshairObj.particleSystem.Stop();
		yield return new WaitForSeconds(.3f);
		redCrosshairObj.particleSystem.Play();
		greenCrosshairObj.particleSystem.Stop();
		canShoot = false;
	}
}
