using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.TestTools;

public class RGBotTests
{
    [UnityTest]
    public IEnumerator RunBotTest()
    {
        yield return new WaitForFixedUpdate();
        Assert.AreEqual(1, 1);
    }
    
}
