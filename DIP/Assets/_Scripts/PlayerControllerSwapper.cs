using UnityEngine;


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

        switch (controller)
        {
            case Controller.VR:
                VRController.SetActive(true);
                break;
            case Controller.NonVR:
                nonVRController.SetActive(true);
                break;
            case Controller.Observer:
                observerController.SetActive(true);
                break;
        }
    }
}