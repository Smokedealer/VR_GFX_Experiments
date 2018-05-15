using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.XR;
using VRTK;


public class PlayerControllerSwapper : MonoBehaviour
{
    private List<VRTK_UICanvas> canvases;
    
    public GameObject VRController;
    public GameObject nonVRController;
    public GameObject observerController;
    public GameObject defaultCamera;

    public Controller activeController;
    
    public Controller swapFrom;
    public Controller swapTo;

    public enum Controller
    {
        VR,
        NonVR,
        Observer,
        SimpleCamera
    }

    private void Start()
    {
        FindAllCanvases();
        
        activeController = swapFrom;
        RefreshActive();
    }

    private void FindAllCanvases()
    {
        canvases = new List<VRTK_UICanvas>();
        
        var foundCanvases = FindObjectsOfType<VRTK_UICanvas>();

        foreach (var canvas in foundCanvases)
        {
            canvases.Add(canvas);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Debug.Log("Swapping!");
            activeController = activeController == swapTo ? swapFrom: swapTo;
            RefreshActive();
        }
    }

    public void RefreshActive()
    {
        VRController.SetActive(false);
        nonVRController.SetActive(false);
        observerController.SetActive(false);
        defaultCamera.SetActive(false);
       
        Cursor.visible = false;
        
        switch (activeController)
        {
            case Controller.VR:
                XRSettings.enabled = true;
                VRController.SetActive(true);
                break;
            case Controller.NonVR:
                XRSettings.enabled = false;
                XRSettings.LoadDeviceByName("None");
                nonVRController.SetActive(true);
                break;
            case Controller.Observer:
                XRSettings.LoadDeviceByName("None");
                XRSettings.enabled = false;
                observerController.SetActive(true);
                break;
            case Controller.SimpleCamera:
                XRSettings.enabled = false;
                XRSettings.LoadDeviceByName("None");
                defaultCamera.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.lockState = CursorLockMode.Confined;
                break;
                
        }
        
        ToggleVRTKCanvases();
    }

    private void ToggleVRTKCanvases()
    {
//        Debug.Log( XRSettings.enabled + " . " + XRDevice.) ;
        
        foreach (var canvas in canvases)
        {
            canvas.enabled = VRController.active;
        }
    }
}