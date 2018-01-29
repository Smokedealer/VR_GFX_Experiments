using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using TMPro;

public class PPExperimentController : MonoBehaviour
{
	public PostProcessingProfile processingProfile;
	public Material cameraMetrial;
	public TextMeshPro questionTextDisplay;
	
	private bool effectEnabled = true;
	
	
	
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetButtonDown("SceneSwap"))
		{
			effectEnabled = !effectEnabled;
			processingProfile.ambientOcclusion.enabled = effectEnabled;
		}
	}
}
