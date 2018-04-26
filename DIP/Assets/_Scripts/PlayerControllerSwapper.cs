using UnityEngine;
using UnityEngine.XR;


public class PlayerControllerSwapper : MonoBehaviour
{
    public GameObject VRController;
    public GameObject nonVRController;
    public GameObject observerController;

    public Controller controller;

    public enum Controller
    {
        VR,
        NonVR,
        Observer
    }

    private void Start()
    {
        VRController.SetActive(false);
        nonVRController.SetActive(false);
        observerController.SetActive(false);
        
        RefreshActive();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            controller = controller == Controller.VR ? Controller.NonVR : Controller.VR;
            RefreshActive();
        }
    }

    public void RefreshActive()
    {
        switch (controller)
        {
            case Controller.VR:
                XRSettings.enabled = true;
                VRController.SetActive(true);
                break;
            case Controller.NonVR:
                XRSettings.enabled = false;
                nonVRController.SetActive(true);
                break;
            case Controller.Observer:
                XRSettings.enabled = false;
                observerController.SetActive(true);
                break;
        }
    }

}