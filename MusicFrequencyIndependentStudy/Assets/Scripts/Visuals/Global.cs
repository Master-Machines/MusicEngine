using UnityEngine;
using System.Collections;

public class Global : MonoBehaviour {
	public static int minTimeBetweenBeats = 20000;
	public static AudioClip audioClip;
	public static float[] audioModifiers = new float[8];
	public static float songTotalAverage = 1f;
}
