using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class IntroOutroUI : MonoBehaviour
{
    public string introString = "Welcome to the experiment. The experiment consists of multiple scenes. Each scene has multiple Questions. Choose your anwers as best as you can.";
    public string outroString = "Thank you for your participation.";
    
    public TextMeshProUGUI headline;
    public TextMeshProUGUI sentence;
    public TextMeshProUGUI mainButtonText;

    public Button mainButton;
    public Button backButton;


    private void Start()
    {
        sentence.text = introString;
    }

    public void SetMainButtonText(string text)
    {
        mainButtonText.text = text;
    }

    public void HideBackButton()
    {
        backButton.gameObject.SetActive(false);
    }
}