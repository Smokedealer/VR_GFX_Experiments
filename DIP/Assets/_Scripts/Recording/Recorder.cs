using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Object = System.Object;


public class Recorder : MonoBehaviour
{
	public Transform playerCamera;
	public GameObject leftController;
	public GameObject rightController;
	
	public GameObject playerCameraDummy;
	public GameObject leftControllerDummy;
	public GameObject rightControllerDummy;

	public FileType FileType;

	private Recording recordedData;

	private bool recording;
	private bool replaying;
	
	private int replayIndex = 0;
	
	// Use this for initialization
	void Start () {

		if (ApplicationDataContainer.replay)
		{
			recordedData = ApplicationDataContainer.loadedRecording;
		}
		else
		{
			recordedData = new Recording {fileType = FileType};

//			playerCamera = Camera.main.transform;
		}
		
	}


	public void StartRecording()
	{
		recording = true;
		replaying = false;
	}

	public void StopRecording()
	{
		recording = false;
		recordedData.SaveRecording();
	}

	public void StartReplay()
	{
		recording = false;
		replaying = true;
	}

	public void StopReplay()
	{
		replaying = false;
	}

	public void PauseReplay()
	{
		replaying = !replaying;
	}

	public void SetReplayToFrame(int replayIndex = 0)
	{
		this.replayIndex = replayIndex;
	}
	
	void FixedUpdate ()
	{
		if (recording)
		{
			RecordFrame();
		}	
		
		
		if(replaying)
		{	
			ReplayFrame();
		}

	}

	public void SetType(Object caller)
	{
		if (caller is PPExperimentController) FileType = FileType.PP;
		if (caller is PPCExperimentController) FileType = FileType.PPC;
		if (caller is OOExperimentController) FileType = FileType.OO;
	}

	public int GetRecordingSize()
	{
		return recordedData.cameraPositions.Count;
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

		if (replayIndex >= cameraRecording.Count)
		{
			Debug.Log("No more to replay.");
			replaying = false;
			return;
		}

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

	
}
