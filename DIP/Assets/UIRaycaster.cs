using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public class UIRaycaster : MonoBehaviour {


	[Header("Controller Key Bindings")]
	[Tooltip("Key used to simulate trigger button.")]
	public KeyCode triggerAlias = KeyCode.Mouse1;
	[Tooltip("Key used to simulate grip button.")]
	public KeyCode gripAlias = KeyCode.Mouse0;
	[Tooltip("Key used to simulate touchpad button.")]
	public KeyCode touchpadAlias = KeyCode.Q;
	[Tooltip("Key used to simulate button one.")]
	public KeyCode buttonOneAlias = KeyCode.E;
	[Tooltip("Key used to simulate button two.")]
	public KeyCode buttonTwoAlias = KeyCode.R;
	[Tooltip("Key used to simulate start menu button.")]
	public KeyCode startMenuAlias = KeyCode.F;
	[Tooltip("Key used to switch between button touch and button press mode.")]
	public KeyCode touchModifier = KeyCode.T;
	[Tooltip("Key used to switch between hair touch mode.")]
	public KeyCode hairTouchModifier = KeyCode.H;
	
	private void Start()
	{
		var controllerSDK = VRTK_SDK_Bridge.GetControllerSDK() as SDK_SimController;
		if (controllerSDK != null)
		{
			Dictionary<string, KeyCode> keyMappings = new Dictionary<string, KeyCode>()
			{
				{"Trigger", triggerAlias },
				{"Grip", gripAlias },
				{"TouchpadPress", touchpadAlias },
				{"ButtonOne", buttonOneAlias },
				{"ButtonTwo", buttonTwoAlias },
				{"StartMenu", startMenuAlias },
				{"TouchModifier", touchModifier },
				{"HairTouchModifier", hairTouchModifier }
			};
			controllerSDK.SetKeyMappings(keyMappings);
		}
		
	}

	// Update is called once per frame
	void Update ()
	{
		
	}
}
