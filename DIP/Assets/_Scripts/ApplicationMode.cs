
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.VR;
using UnityEngine.XR;
using VRTK;

public class ApplicationMode : MonoBehaviour
{
    private PlayerControllerSwapper swapper;

    public TextMeshProUGUI modeText;
    public TextMeshProUGUI errorBox;
    
//    private void Awake()
//    {
//        
//    }

    private void Start()
    {
        swapper = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerSwapper>();

        if (!ApplicationDataContainer.initialSetupDone)
        {
            StartCoroutine(TryLoadVR());
            ApplicationDataContainer.initialSetupDone = true;
        }
        else
        {
            swapper.RefreshActive();
        }
        
        ChangeButtonText();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            ModeSwap();
        }
    }

    public void ModeSwap()
    {
        ApplicationDataContainer.runMode = ApplicationDataContainer.runMode == Controller.VR ? Controller.NonVR : Controller.VR;

        if (ApplicationDataContainer.runMode == Controller.VR)
        {
            StartCoroutine(TryLoadVR());
        }
        
        ChangeButtonText();

        swapper.RefreshActive();
    }

    private void ChangeButtonText()
    {
        if (ApplicationDataContainer.runMode == Controller.VR)
        {
            modeText.text = "MODE: VR";
        }
        else
        {
            modeText.text = "MODE: NON-VR";
        }
    }

    IEnumerator TryLoadVR()
    {
        XRSettings.LoadDeviceByName("OpenVR");
        yield return null;
        if (string.IsNullOrEmpty(XRSettings.loadedDeviceName))
        {
            errorBox.text = "It seems like there is no virtual reality device connected to the computer.";
            // Load Non-VR Input Controls
            // Stops trying to load something that is not there
            XRSettings.LoadDeviceByName("None");
            ApplicationDataContainer.runMode = Controller.NonVR;
        }
        else
        {
            errorBox.text = "";
            // Load VR Input Controls
            XRSettings.enabled = true;
            ApplicationDataContainer.runMode = Controller.VR;
        }
        
        ChangeButtonText();

        
        swapper.RefreshActive();
    }
}