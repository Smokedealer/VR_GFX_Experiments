using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ExperimentEffects {
    private Dictionary<string, string> effects;

    ExperimentEffects instance;

    public ExperimentEffects getInstance()
    {
        if (instance == null)
        {
            instance = new ExperimentEffects();
        }

        return instance;
    }

    private ExperimentEffects()
    {
        effects = new Dictionary<string, string>();
        PopulateEffects();
    }

    private void PopulateEffects()
    {
        //TODO load from file
        effects.Add("Normal Mapping", "_BumpMap");
        effects.Add("Ambient Occlusion", "_OcclusionMap");
        effects.Add("Main Texture", "_MainTex");
    }

    public string[] GetAvailableEffects()
    {
        return effects.Keys.ToArray();
    }

    public string GetValue(string key)
    {
        return effects[key];
    }


}
