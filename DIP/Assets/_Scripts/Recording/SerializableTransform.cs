using System;
using UnityEngine;

[Serializable]
public class SerializableTransform
{
    private float[] data;

    public SerializableTransform(Vector3 data)
    {
        this.data = new float[3];

        this.data[0] = data.x;
        this.data[1] = data.y;
        this.data[2] = data.z;
    }
    
    
    public SerializableTransform(Quaternion data)
    {
        this.data = new float[4];

        this.data[0] = data.x;
        this.data[1] = data.y;
        this.data[2] = data.z;
        this.data[3] = data.w;
    }

    public Vector3 GetVector3()
    {
        return new Vector3(data[0], data[1], data[2]);
    }
    
    
    public Quaternion GetQuaternion()
    {
        Quaternion quaternion = Quaternion.identity;

        quaternion.x = data[0];
        quaternion.y = data[1];
        quaternion.z = data[2];
        quaternion.w = data[3];

        return quaternion;
    }
}