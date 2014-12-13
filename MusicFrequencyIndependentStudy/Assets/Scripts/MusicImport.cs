using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using NAudio;
using NAudio.Wave;

public class MusicImport : MonoBehaviour {

	public Transform[] cubies;
	public AudioSource source;
	private bool isReady = false;
	
	
	IEnumerator Start () {
		source = audio;
		DirectoryInfo dir = new DirectoryInfo("c:/");
		
		string[] topDirectories = Directory.GetLogicalDrives();
		for(int i = 0; i < topDirectories.Length; i++) {
			Debug.Log(topDirectories[i]);
		}
		yield return new WaitForSeconds(.01f);
		
		/*
		FileInfo[] info = dir.GetFiles("*.mp3", SearchOption.AllDirectories);
		List<FileInfo> songList = new List<FileInfo>();
		
		foreach(FileInfo f in info) {
			if(!f.IsReadOnly && f.Length > 1000000 && f.Length < 20000000) {
				Debug.Log(f.Name);
				songList.Add(f);
			}
		}
		
		FileInfo file = songList[1];

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
		//Application.LoadLevel("vr_test");
		isReady = true;
		*/
	}
	
	public static IEnumerator ImportAudio(FileInfo file) {
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
		a.name = file.Name;
		Global.audioClip = a;
		GameObject.Find("Menu Tracker").SendMessage("ImportComplete");
	}
	
	public static FileInfo[] FindMusicInDirectory(DirectoryInfo d) {
		FileInfo[] info = d.GetFiles("*.mp3");
		List<FileInfo> songList = new List<FileInfo>();
		foreach(FileInfo f in info) {
			if(!f.IsReadOnly && f.Length > 1000000 && f.Length < 20000000) {
				songList.Add(f);
			}
		}
		return songList.ToArray();
	}
	
	public static string[] GetLogicalDrives() {
		return Directory.GetLogicalDrives();
	}
	
	public static DirectoryInfo[] GetSubDirectories(DirectoryInfo d) {
		List<DirectoryInfo> directories = new List<DirectoryInfo>();
		try {
			DirectoryInfo[] ds = d.GetDirectories();
			foreach(DirectoryInfo dInfo in ds) {
				try {
					FileInfo[] files = dInfo.GetFiles();
					directories.Add(dInfo);
				} catch (System.Exception e) {
					// do something and return
				}
				
			}
		}catch (System.Exception e) {
			// do something and return
		}
		
		return directories.ToArray();
	}
	
	public static void Mp3ToWav(string mp3File, string outputFile)
	{
		using (Mp3FileReader reader = new Mp3FileReader(mp3File))
		{
			WaveFileWriter.CreateWaveFile(outputFile, reader);
		}
	}
}
