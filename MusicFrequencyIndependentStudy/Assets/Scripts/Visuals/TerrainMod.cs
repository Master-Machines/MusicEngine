using UnityEngine;
using System.Collections;
using System.Threading;

public class TerrainMod : MonoBehaviour {
	public Terrain terrain;
	public float currentAmplitude;
	// X = x position in height map, Y = x position in height map, Z is radius, and W is height
	public Vector3[] points;
	private int xRes;
	private int zRes;
	private Vector2 center;
	private float[,] heights;
	public bool canLoop = true;
	// Use this for initialization
	
	/*
	public ThreadStart StartWork () {
		 xRes = terrain.terrainData.heightmapWidth;
		zRes = terrain.terrainData.heightmapHeight;
		center.x = (float)xRes / 2f;
		center.y = (float)zRes / 2f;
		heights = terrain.terrainData.GetHeights(0, 0, xRes, zRes);
		while(canLoop) {
			for(int x = 0; x < xRes; x++) {
				for(int z = 0; z < zRes; z++) {
					heights[x, z] = GetHeight(x, z);
				}
			}
			terrain.terrainData.SetHeights(0, 0, heights);
			currentAmplitude *= 1.002f;
			Debug.Log("test");
		}
		
	}
	*/
	// Update is called once per frame
	void Update () {
	}
	
	float GetHeight(int x, int z) {
		float height = 0f;
		for(int i = 0; i < points.Length; i++) {
			float xDistance = Mathf.Abs((float)x - points[i].x);
			float zDistance = Mathf.Abs ((float)z - points[i].y);
			if(xDistance <= points[i].z && zDistance <= points[i].z) {
				float newHeight = Vector2.Distance(center, new Vector2(points[i].x, points[i].y)) / center.x;
				if(height < newHeight) {
					height = newHeight;
				}
			}
		}
		return height * currentAmplitude;
	}
}
