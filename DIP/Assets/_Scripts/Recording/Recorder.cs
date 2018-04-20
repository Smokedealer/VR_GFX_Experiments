using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public class Recorder : MonoBehaviour
{
	public Transform playerCamera;
	public GameObject leftController;
	public GameObject rightController;
	
	public GameObject playerCameraDummy;
	public GameObject leftControllerDummy;
	public GameObject rightControllerDummy;

	public Experiment Experiment;

	private Recording recordedData;

	private bool recording;
	
	// Use this for initialization
	void Start () {
		recordedData = new Recording();
		recording = true;

		playerCamera = Camera.main.transform;
	}


	private int replayIndex = 0;
	
	void FixedUpdate ()
	{
		if (recording)
		{
			RecordFrame();	
			
			if (recordedData.cameraPositions.Count > 1000)
			{
				Debug.Log("Saving recording");
				SaveRecording();
				recording = false;
				Debug.Log("Loading recording");
				LoadRecording();
			}
		}
		else
		{	
			ReplayFrame();
		}

	}

	private void ReplayFrame()
	{
		if (recordedData == null)
		{
			Debug.LogError("No data to replay");
			return;
		}
		
		var cameraRecording = recordedData.cameraPositions;
		var leftControllerRecording = recordedData.leftControllerPositions;
		var rightControllerRecording = recordedData.rightControllerPositions;
		
		if (replayIndex >= cameraRecording.Count) return;

		PointInTime pointInTime = cameraRecording[replayIndex];
		playerCameraDummy.transform.position = pointInTime.GetPosition();
		playerCameraDummy.transform.rotation = pointInTime.GetRotation();

		pointInTime = leftControllerRecording[replayIndex];
		leftControllerDummy.transform.position = pointInTime.GetPosition();
		leftControllerDummy.transform.rotation = pointInTime.GetRotation();

		pointInTime = rightControllerRecording[replayIndex];
		rightControllerDummy.transform.position = pointInTime.GetPosition();
		rightControllerDummy.transform.rotation = pointInTime.GetRotation();

		replayIndex++;
	}


	private void RecordFrame()
	{
		recordedData.cameraPositions.Add(new PointInTime(playerCamera.position, playerCamera.rotation));
		if (leftController != null) recordedData.leftControllerPositions.Add(new PointInTime(leftController.transform.position, leftController.transform.rotation));
		if (rightController != null) recordedData.rightControllerPositions.Add(new PointInTime(rightController.transform.position, rightController.transform.rotation));
	}

	public void SaveRecording(string fileName = "testRecording.rec")
	{
		string directory = "Recordings";
		string filePath = directory + "/" + fileName;
		if(!Directory.Exists(directory))
		{
			Directory.CreateDirectory(directory);
		}
		
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream stream = new FileStream(filePath, FileMode.Create);		
		
		binaryFormatter.Serialize(stream, recordedData);
		stream.Close();
	
	}

	public Recording LoadRecording(string filePath = "Recordings/testRecording.rec")
	{
		
		if (File.Exists(filePath))
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			FileStream stream = new FileStream(filePath, FileMode.Open);			
			
			var recording = binaryFormatter.Deserialize(stream) as Recording;
			stream.Close();

			return recording;
		}
		else
		{
			Debug.LogError("File " + filePath + " does not exist.");
			return null;
		}
	}
}
