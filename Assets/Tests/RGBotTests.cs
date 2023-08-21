using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.TestTools;

public class RGBotTests
{
    [UnityTest]
    public IEnumerator RunBotTest()
    {
        string botId = Environment.GetEnvironmentVariable("RG_BOT");
        string rgApiKey = Environment.GetEnvironmentVariable("RG_API_KEY");

        yield return new WaitForFixedUpdate();
        Assert.AreEqual(botId, "32");
        Assert.AreEqual(rgApiKey, "ABC");
    }
    
}
