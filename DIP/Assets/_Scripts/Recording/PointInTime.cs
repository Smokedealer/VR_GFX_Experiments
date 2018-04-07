using System;
using UnityEngine;

[Serializable]
public class PointInTime
{
    private readonly Float3Holder position;
    private readonly Float3Holder rotation;

    
    public PointInTime(Vector3 position, Quaternion quaternion)
    {
        this.position = new Float3Holder(position);
        this.rotation = new Float3Holder(quaternion);
    }

    public Vector3 GetPosition()
    {
        return position.GetVector3();
    }

    public Quaternion GetRotation()
    {
        return rotation.GetQuaternion();
    }
}