using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VRTK;

public class UIRaycaster : MonoBehaviour
{
	private Camera _camera;
	private Vector2 pos;
	
	private void Start()
	{
		_camera = Camera.current;
		pos = new Vector2(Screen.width/2, Screen.height/2);		
	}

	// Update is called once per frame
	void Update ()
	{
		RaycastWorldUI();	
	}
	
	void RaycastWorldUI(){
		if(Input.GetMouseButtonDown(0)){
			
			Debug.Log("Clicked");
			
			PointerEventData pointerData = new PointerEventData(EventSystem.current);
 
			pointerData.position = pos;

			Debug.Log(pointerData);
 
			List<RaycastResult> results = new List<RaycastResult>();
			EventSystem.current.RaycastAll(pointerData, results);
			Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)));
 
			if (results.Count > 0) {
				string dbg = "Root Element: {0} \n GrandChild Element: {1}";
				Debug.Log(string.Format(dbg, results[results.Count-1].gameObject.name,results[0].gameObject.name));
		
				//WorldUI is my layer name
				if (results[0].gameObject.layer == LayerMask.NameToLayer("UI")){ 
					dbg = "Root Element: {0} \n GrandChild Element: {1}";
					Debug.Log(string.Format(dbg, results[results.Count-1].gameObject.name,results[0].gameObject.name));
					//Debug.Log("Root Element: "+results[results.Count-1].gameObject.name);
					//Debug.Log("GrandChild Element: "+results[0].gameObject.name);
					results.Clear();
				} 
			}
		}
	}
}
