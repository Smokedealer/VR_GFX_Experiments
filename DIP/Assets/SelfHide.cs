using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfHide : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (ApplicationDataContainer.runMode == Controller.NonVR)
		{
			enabled = false;
		}
	}
	

}
