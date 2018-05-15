using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObserverUIController : MonoBehaviour
{

	public Recorder recorder;

	public Slider slider;

	public Button playPauseButton;
	public Text buttonText;

	public TextMeshProUGUI recordingProgressText;

	private int maxFrames;
	private int currentFrame;

	private bool playing;
	
	// Use this for initialization
	void Start () {
		if (!recorder)
		{
			enabled = false;
		}
		else
		{
			maxFrames = recorder.GetRecordingSize();
			slider.maxValue = maxFrames;
			slider.onValueChanged.AddListener(delegate {ValueChangeCheck(); });
			playing = true;
		}
	}

	private void ValueChangeCheck()
	{
		recorder.SetReplayToFrame((int)slider.value);
		recorder.ReplayFrame();
	}

	// Update is called once per frame
	private void FixedUpdate()
	{
		slider.value = recorder.GetCurrentFrameIndex();
		recordingProgressText.text = slider.value + "/" + maxFrames;
	}

	public void PlayPauseClicked()
	{
		playing = !playing;
		recorder.PauseReplay();

		buttonText.text = playing ? "❚❚" : "►";
	}
}
