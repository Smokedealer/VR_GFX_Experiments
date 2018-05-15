using System;
using UnityEngine;

/// <summary>
/// Serializable vontainer for position and rotation.
///
/// Used withing the recorder class.
/// <see cref="Recorder"/>
/// 
/// </summary>
[Serializable]
public class PointInTime
{
    /// <summary>
    /// Serializable position (since Vector3 is not serializable)
    /// </summary>
    private readonly SerializableTransform position;
    
    
    /// <summary>
    /// Serializable rotation (since Quaternion is not serializable)
    /// </summary>
    private readonly SerializableTransform rotation;

    /// <summary>
    /// Constructor that takes position and rotation of a Transform
    /// </summary>
    /// <param name="position">Transform position</param>
    /// <param name="quaternion">Transform rotatino</param>
    public PointInTime(Vector3 position, Quaternion quaternion)
    {
        this.position = new SerializableTransform(position);
        this.rotation = new SerializableTransform(quaternion);
    }

    /// <summary>
    /// Position getter. It creates Vector3 back from serialized version.
    /// </summary>
    /// <returns>Vector3 from serialized data</returns>
    public Vector3 GetPosition()
    {
        return position.GetVector3();
    }

    /// <summary>
    /// Rotation getter.
    /// </summary>
    /// <returns>Quaternion from stored data</returns>
    public Quaternion GetRotation()
    {
        return rotation.GetQuaternion();
    }
}