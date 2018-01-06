using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void startGame()
    {
        SceneManager.LoadScene(1);
    }

    public void quitApplication()
    {
        //TODO Exit app
        Debug.Log("Application exit");
    }
}
