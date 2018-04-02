using UnityEngine;

public class PointInTime
{
    private Vector3 _transform;
    private Quaternion _quaternion;

    public PointInTime(Vector3 transform, Quaternion quaternion)
    {
        _transform = transform;
        _quaternion = quaternion;
    }
}