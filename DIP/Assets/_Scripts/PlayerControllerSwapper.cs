using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using VRTK;


public class PlayerControllerSwapper : MonoBehaviour
{
    private List<VRTK_UICanvas> canvases;
    private List<VRTK_DestinationPoint> teleportPads;
    
    public GameObject VRController;
    public GameObject nonVRController;
    public GameObject observerController;
    public GameObject defaultCamera;

    public Controller activeController;

    private void Awake()
    {
        FindAllCanvases();
        FindAllTeleportPads();
    }

    public void FindAllCanvases()
    {
        canvases = new List<VRTK_UICanvas>();
        
        var foundCanvases = FindObjectsOfType<VRTK_UICanvas>();

        foreach (var canvas in foundCanvases)
        {
            canvases.Add(canvas);
        }
    }
    
    public void FindAllTeleportPads()
    {
        teleportPads = new List<VRTK_DestinationPoint>();
        
        var foundPads = FindObjectsOfType<VRTK_DestinationPoint>();

        foreach (var pad in foundPads)
        {
            teleportPads.Add(pad);
        }
    }


    public void RefreshActive()
    {
        activeController = ApplicationDataContainer.runMode;

        if (ApplicationDataContainer.replay && SceneManager.GetActiveScene().buildIndex != 0)
        {
            activeController = Controller.Observer;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 0 && activeController == Controller.NonVR)
        {
            activeController = Controller.SimpleCamera;
        }
        
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
                UnlockMouseAndDisableVR();
                observerController.SetActive(true);
                break;
            case Controller.SimpleCamera:
                UnlockMouseAndDisableVR();
                defaultCamera.SetActive(true);
                break;
                
        }
        
        ToggleVRCanvasesAndTeleportPads();
    }

    private void UnlockMouseAndDisableVR()
    {
        XRSettings.LoadDeviceByName("None");
        XRSettings.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void ToggleVRCanvasesAndTeleportPads()
    {
        
        foreach (var canvas in canvases)
        {
            canvas.enabled = VRController.active;
        }

        foreach (var pad in teleportPads)
        {
            pad.gameObject.active = VRController.active;
        }
    }
}