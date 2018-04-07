using System.Collections.Generic;

public class Recording
{
    public Experiment Experiment;
    public List<PointInTime> playerRecording;

    public Recording()
    {
        playerRecording = new List<PointInTime>();
    }

    public void Add(PointInTime pointInTime)
    {
        playerRecording.Add(pointInTime);
    }

}