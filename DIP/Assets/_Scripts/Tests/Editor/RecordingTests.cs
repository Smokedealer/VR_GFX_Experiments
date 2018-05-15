using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class RecordingTests {

	[Test]
	public void Float3Holder_PositionTest()
	{
		Vector3 position = new Vector3(5f, 6f, 7f);
		SerializableTransform holder = new SerializableTransform(position);

		Vector3 saved = holder.GetVector3();

		Assert.AreEqual(position, saved);
	}
	
	[Test]
	public void Float3Holder_QuaternionTest()
	{
		Quaternion quaternion = Quaternion.identity;
		quaternion.x = 5f;
		quaternion.y = 6f;
		quaternion.z = 7f;
		SerializableTransform holder = new SerializableTransform(quaternion);

		Quaternion saved = holder.GetQuaternion();

		Assert.AreEqual(quaternion, saved);
	}
	
	
}
