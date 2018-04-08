using System;
using System.Collections.Generic;

[Serializable]
public class Recording
{
    public Experiment Experiment;
    public List<PointInTime> cameraPositions;
    public List<PointInTime> leftControllerPositions;
    public List<PointInTime> rightControllerPositions;

    public Recording()
    {
        cameraPositions = new List<PointInTime>();
        leftControllerPositions = new List<PointInTime>();
        rightControllerPositions = new List<PointInTime>();
    }

}