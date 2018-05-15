using System;
using System.Xml.Serialization;
using UnityEngine;


public class EffectSettings
{    
    [XmlElement(ElementName = "PropertyName")]
    public string propertyName;

    [XmlElement(ElementName = "PropertyType")]
    public string propertyType;
    
    [XmlElement(ElementName = "PropertyValue")]
    public string propertyValue;
    
    
    public void SetPropertyByType(EffectSettings effectSettings, Material effectMaterial)
    {
        
        switch (effectSettings.propertyType)
        {
            case "texture":
                if(float.Parse(effectSettings.propertyValue) == 0) effectMaterial.SetTexture(effectSettings.propertyName, null);
                effectMaterial.SetFloat(effectSettings.propertyName, float.Parse(effectSettings.propertyValue));
                break;
            case "integer":
                effectMaterial.SetInt(effectSettings.propertyName, int.Parse(effectSettings.propertyValue));
                break;
            case "float":
                effectMaterial.SetFloat(effectSettings.propertyName, float.Parse(effectSettings.propertyValue));
                break;
            case "color":
                var color = ParseColor(effectSettings);
                effectMaterial.SetColor(effectSettings.propertyName, color);
                break;
        }
    }
    
    private static Color ParseColor(EffectSettings effectSettings)
    {
        var colorString = effectSettings.propertyValue;

        const char delimiter = ',';

        var substrings = colorString.Split(delimiter);

        Color color = new Color();

        for (int i = 0; i <= 4; i++)
        {
            color[i] = float.Parse(substrings[i]);
        }

        return color;
    }
}
