using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public class Recorder : MonoBehaviour
{
	public GameObject player;

	public GameObject playerDummy;

	public Experiment Experiment;

	[SerializeField] private Recording recording;

	private bool replaying = false;
	
	// Use this for initialization
	void Start () {
		recording = new Recording();
		replaying = false;
	}


	// Update is called once per frame

	private int index = 0;
	
	void FixedUpdate ()
	{
		var playerRecording = recording.playerRecording;

		if (!replaying)
		{
			RecordFrame(playerRecording);	
			
			if (playerRecording.Count > 300)
			{
				Debug.Log("Saving recording");
//				SaveRecording();
//				playerRecording.Clear();
				replaying = true;
			}
		}
		else
		{
			if (index > playerRecording.Count) return;
			
			PointInTime pointInTime = playerRecording[index++];
			playerDummy.transform.position = pointInTime.GetPosition();
			playerDummy.transform.rotation = pointInTime.GetRotation();
		}
		
		
		
	}

	private void RecordFrame(List<PointInTime> playerRecording)
	{
		playerRecording.Add(new PointInTime(player.transform.position, player.transform.rotation));

		if (playerRecording.Count % 1000 == 0)
		{
			Debug.Log("Recording size: " + playerRecording.Count);
		}
	}

	public void SaveRecording()
	{
		string directory = "Recordings";
		string filePath = directory + "/" + recording.GetHashCode() + ".rec";
		if(!Directory.Exists(directory))
		{
			Directory.CreateDirectory(directory);
		}
		
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream stream = new FileStream(filePath, FileMode.CreateNew);		
		
		binaryFormatter.Serialize(stream, recording);
		stream.Close();
	
	}

	public void LoadRecording(string filePath)
	{
		
		if (File.Exists(filePath))
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			FileStream stream = new FileStream(filePath, FileMode.Create);			
			
			
		}
		else
		{
			Debug.LogError("File " + filePath + " does not exist.");
			
		}
	}
}
