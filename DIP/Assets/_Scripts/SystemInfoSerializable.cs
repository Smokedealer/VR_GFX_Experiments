
using System;
using System.Xml.Serialization;
using UnityEngine;

[Serializable]
public class SystemInfoSerializable
{
    [XmlElement(ElementName = "GPU")] public string gpuName;

    [XmlElement(ElementName = "Driver")] public string driver;

    [XmlElement(ElementName = "CPU")] public string cpuName;
}