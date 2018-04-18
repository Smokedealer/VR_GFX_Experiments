using UnityEngine;

public abstract class AbstractExperimentController : MonoBehaviour
{
    public abstract void StartExperiment();
    
    public abstract void NextTest();
    
    public abstract void NextQuestion();

    public abstract void EndExperiment();
}