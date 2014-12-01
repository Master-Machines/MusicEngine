using UnityEngine;
using System.Collections;
using System.IO;
using NAudio;
using NAudio.Wave;

public class MusicImport : MonoBehaviour {

	public FFTWindow FFTMode;
	public int sampleCount;
	public Transform[] cubies;
	float[] samples;
	public AudioSource source;
	bool isReady = false;
	
	IEnumerator Start () {
		source = audio;
		samples = new float[sampleCount];
		DirectoryInfo dir = new DirectoryInfo("C:/Users");
		FileInfo[] info = dir.GetFiles("*.mp3", SearchOption.AllDirectories);
		
		foreach(FileInfo f in info) {
			Debug.Log(f.Name);
		}
		
		FileInfo file = info[1];

		char[] chars = new char[3] {file.Name[file.Name.Length - 3], file.Name[file.Name.Length - 2], file.Name[file.Name.Length - 1]};
		
		string ext = new string(chars);
		
		if(file.Name[file.Name.Length - 3] == "mp3"[0])
		{
			Directory.CreateDirectory(System.IO.Path.GetTempPath() + @"\Imported_Songs");
			Mp3ToWav(file.FullName, System.IO.Path.GetTempPath() + @"\Imported_Songs\currentsong.wav");
			ext = "wav";
		}
		else
		{
			Directory.CreateDirectory(System.IO.Path.GetTempPath() + @"\Imported_Songs");
			File.WriteAllBytes(System.IO.Path.GetTempPath() + @"\Imported_Songs\currentsong." + ext, File.ReadAllBytes(file.FullName));
		}
		
		WWW www = new WWW("file://" + System.IO.Path.GetTempPath() + @"\Imported_Songs\currentsong." + ext);
		AudioClip a = www.GetAudioClip(false, false);
		
		while(!a.isReadyToPlay)
		{
			Debug.Log("still in loop");
			yield return www; 
		}
		
		
		source.clip = a;
		source.Play();
		Global.audioClip = a;
		Application.LoadLevel("vr_test");
		isReady = true;
	}
	
	public static void Mp3ToWav(string mp3File, string outputFile)
	{
		using (Mp3FileReader reader = new Mp3FileReader(mp3File))
		{
			WaveFileWriter.CreateWaveFile(outputFile, reader);
		}
	}
}
