using System;
using System.Collections.Generic;
using System.Xml.Serialization;


public class Question
{
    [XmlElement(ElementName = "Text")]
    public string questionText;
    
    [XmlElement(ElementName = "ExperimentPart")]
    public int experimentPart;
    
    [XmlArray(ElementName = "Options")]
    [XmlArrayItem("Option")]
    public List<string> questionOptions = new List<string>();
    
    [XmlElement(ElementName = "Answer")]
    public int answerIndex;

    public void AddLeftRightUndecidedOptions()
    {
        questionOptions.Add("Left");
        questionOptions.Add("Can't decide");
        questionOptions.Add("Right");
    }

    public void AddRating(int count)
    {
        for (int i = 0; i < count; i++)
        {
            questionOptions.Add(i.ToString());
        }
    }

    public void setBasicQuestion()
    {
        AddLeftRightUndecidedOptions();
        questionText = "Which of the two objects is more visually appealing";
    }
}