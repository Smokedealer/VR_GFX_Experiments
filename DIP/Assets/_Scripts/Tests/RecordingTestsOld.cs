using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.TestTools;

namespace Tests
{
    public class RecordingTests
    {
	    [UnityTest]
	    public IEnumerator Float3HolderPositionTest()
	    {
		    Vector3 position = new Vector3(5f,5f,5f);
		    Float3Holder holder = new Float3Holder(position);

		    Vector3 saved = holder.GetVector3();

		    yield return null;
		    
		    Assert.AreEqual(position, saved);
	    }
	
    }
}

