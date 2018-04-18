using UnityEngine;
using TMPro;

public class IntroOutroUI : MonoBehaviour
{
    public string introString = "Welcome to the experiment. The experiment consists of multiple scenes. Each scene has multiple Questions. Choose your anwers as best as you can.";
    public string outroString = "Thank you for your participation.";
    
    public TextMeshProUGUI headline;
    public TextMeshProUGUI sentence;
    public TextMeshProUGUI mainButtonText;


    private void Start()
    {
        sentence.text = introString;
    }
}