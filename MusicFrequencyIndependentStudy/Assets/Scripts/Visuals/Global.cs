using UnityEngine;
using System.Collections;
using System.IO;

public class Global : MonoBehaviour {
	public static int minTimeBetweenBeats = 20000;
	public static AudioClip audioClip;
	public static float[] audioModifiers = new float[8]{1f,1f,1f,1f,1f,1f,1f,1f};
	public static float songTotalAverage = 1f;
	public static int totalBeats = 10;
	public static int beatsHit = 5;
	public static int longestStreak = 0;
	public static DirectoryInfo lastDirectory;
	public static string lastDrive;
	public static int difficulty = 1;
}
