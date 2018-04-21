using UnityEngine;

public class PPCExperimentController : PPExperimentController
{

    private CustomShaderScript customShaderScript;
    
    private void Start()
    {
//        customShaderScript.enabled = true;
        Debug.Log("Yo waddup");
    }

    public override void SceneSwap()
    {
        Debug.Log("Yo waddup");
    }
}