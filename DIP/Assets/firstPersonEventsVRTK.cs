using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.EventSystems;
using VRTK;

public class firstPersonEventsVRTK : MonoBehaviour
{

	public VRTK_ControllerEvents events;
	public VRTK_UIPointer pointer;
	
	// Use this for initialization
	void Start ()
	{
		events.enabled = true;
		pointer.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	
	}
}
