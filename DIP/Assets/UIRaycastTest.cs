using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIRaycastTest : MonoBehaviour
{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		var pointer = new PointerEventData(EventSystem.current);

		pointer.position = transform.position;
		
		var raycastResults = new List<RaycastResult>();
		
		EventSystem.current.RaycastAll(pointer, raycastResults);
		
		Debug.DrawRay(pointer.position, pointer.position*5);
	}
}
