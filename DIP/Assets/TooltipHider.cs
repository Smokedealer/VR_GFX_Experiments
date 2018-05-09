using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class TooltipHider : MonoBehaviour
{
	private VRTK_ControllerTooltips _tooltips;


	public bool visible;
	// Use this for initialization
	void Start ()
	{
		visible = true;
		_tooltips = GetComponent<VRTK_ControllerTooltips>();
	}


	public void Toggle()
	{
		visible = !visible;
		_tooltips.ToggleTips(visible);
	}
	
}
