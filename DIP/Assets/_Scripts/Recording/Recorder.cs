using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recorder : MonoBehaviour
{
	public GameObject player;

	private List<PointInTime> playerRecording;
	
	// Use this for initialization
	void Start () {
		playerRecording = new List<PointInTime>();	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		playerRecording.Add(new PointInTime(player.transform.position, player.transform.rotation));

		if (playerRecording.Count % 1000 == 0)
		{
			Debug.Log("Recording size: " + playerRecording.Count);
		}
	}
}
